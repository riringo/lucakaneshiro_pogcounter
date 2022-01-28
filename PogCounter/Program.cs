using PogCounter.Helper;
using PogCounter.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace PogCounter
{
    class Program
    {
        public static Analyzer analyzer = new Analyzer();
        public static MetadataReader dataReader = new MetadataReader();
        public static DataTable metadataTable = new DataTable();
        public static DataPrinter resultsPrinter = new DataPrinter();
        public static DataTable summaryTable = new DataTable();
        static void Main(string[] args)
        {

            //Check that path exists
            string TranscriptsPath = "../../../Transcripts";
            string outputFile = GetOutputFileName();
            string streamDataFile = "LucaKaneshiroStreams.csv";

            Console.WriteLine("Welcome to the Word Counter. For full instructions on how to use this program, please refer to the ReadMe.");
            Console.WriteLine($"Reading files from {Path.GetFullPath(TranscriptsPath)} & outputting overall results to {Path.GetFullPath(outputFile)}");
            Console.WriteLine("Each individual file's results will be saved separately in Results folder.");
            Console.WriteLine($"Reading stream data from {Path.GetFullPath(TranscriptsPath)} through {streamDataFile}");

            if (Directory.Exists("../../../Results"))
            {
                Directory.Delete("../../../Results", true);
                Directory.CreateDirectory("../../../Results");
            }


            metadataTable = dataReader.ConvertCsvToDataTable(Path.Combine(TranscriptsPath, streamDataFile));
            //dataReader.ShowData(streamDataTable);

            summaryTable = CreateOverallTable();

            var files = GetTranscripts(TranscriptsPath);
            var transcripts = new List<Transcript>();
            foreach (var file in files)
            {
                var completedTranscript = AnalyzeTranscript(file);
                transcripts.Add(completedTranscript);
                AddTranscriptToSummaryDT(completedTranscript);
            }


            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;


            try
            {
                ostrm = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Cannot open {outputFile} for writing");
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(writer);
            Console.WriteLine($"__Last Update: {DateTime.UtcNow} UTC__");
            Console.WriteLine();
            Console.WriteLine($"## TOTALS of {files.Length} Transcripts");
            Console.WriteLine();
            analyzer.GetTotal();
            Console.WriteLine();
            analyzer.PrintDictionary();
            Console.WriteLine();
            analyzer.PrintLoveDictionary();
            Console.WriteLine();
            analyzer.GetLoveTotal();

            Console.WriteLine();
            Console.WriteLine("## Overall Summary");
            Console.WriteLine();
            dataReader.ShowData(summaryTable);
            Console.WriteLine();
            Console.WriteLine("## Individual Results:");
            Console.WriteLine();

            foreach(var t in transcripts) 
            {
                resultsPrinter.PrintTranscript(t);
            }

            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
            Console.WriteLine("Analysis completed.");


           // CombineMultipleFilesIntoSingleFile("../../../Results", "*.md", GetCombinedOutputFileName());

        }

        private static void AddTranscriptToSummaryDT(Transcript completedTranscript) 
        {
            DataRow dr = summaryTable.NewRow();

            var metadataColumn = new string[6];
            metadataColumn[0] = completedTranscript.GetVidTitle();
            metadataColumn[1] = completedTranscript.GetStreamDate().ToString();
            metadataColumn[2] = completedTranscript.GetRunTimeMinutes().ToString();
            metadataColumn[3] = completedTranscript.GetYTLink();
            metadataColumn[4] = completedTranscript.GetPogDensity().ToString();
            metadataColumn[5] = completedTranscript.GetPogPerMinute().ToString();
           
            dr = summaryTable.NewRow();
            dr.ItemArray = metadataColumn;
            summaryTable.Rows.Add(dr);
        }

        private static DataTable CreateOverallTable() 
        {
            DataTable dtData = new DataTable();
            string[] rowValues = null;

            //Creating columns
            dtData.Columns.Add("StreamTitle");
            dtData.Columns.Add("StreamDate");
            dtData.Columns.Add("Runtime");
            dtData.Columns.Add("YoutubeLink");
            dtData.Columns.Add("PogDensity");
            dtData.Columns.Add("PogPerMinute");

            return dtData;

        }

        private static void CombineMultipleFilesIntoSingleFile(string inputDirectoryPath, string inputFileNamePattern, string outputFilePath)
        {
            string[] inputFilePaths = Directory.GetFiles(inputDirectoryPath, inputFileNamePattern);
            Console.WriteLine("Number of files: {0}.", inputFilePaths.Length);
            using (var outputStream = File.OpenWrite(outputFilePath))
            {
                foreach (var inputFilePath in inputFilePaths)
                {
                    using (var inputStream = File.OpenRead(inputFilePath))
                    {
                        // Buffer size can be passed as the second argument.
                        inputStream.CopyTo(outputStream);
                    }
                    Console.WriteLine("The file {0} has been processed.", inputFilePath);
                }
            }
        }

        public static string GetOutputFileName()
        {
            string fileName = "PogResults";
            int duplicativeIndex = 1;
            string outputFile = $"../../../{fileName}.md";

            while (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }
            return outputFile;
        }

        public static string GetCombinedOutputFileName()
        {
            string fileName = "AllPogResults";
            int duplicativeIndex = 1;
            string outputFile = $"../../../{fileName}.md";

            while (File.Exists(outputFile))
            {
                outputFile = $"../../../{fileName}{duplicativeIndex}.md";
                duplicativeIndex++;
            }
            return outputFile;
        }

        public static string[] GetTranscripts(string transcriptPath)
        {
            var files = Directory.GetFiles(transcriptPath, "*.txt");
            Console.WriteLine($"Found {files.Length} transcripts");
            return files;
        }

        public static Transcript AnalyzeTranscript(string filePath)
        {
            var fileParts = filePath.Split("\\");
            var fileName = fileParts[fileParts.Length - 1];

            var rowData = metadataTable.Select($"textFile = '{fileName}'")[0];
            if (rowData == null)
            {
                Console.WriteLine($"Cannot find {fileName} in the Metadata information. Please update LucaKaneshiroStreams.csv file.");
                return null; 
            }
            else
            {

                var currentTranscript = new Transcript(filePath, rowData);
                var pogDictionary = analyzer.AnalyzeText(currentTranscript);
                currentTranscript.SetPogDictionary(pogDictionary);
                resultsPrinter.PrintSingleTranscript(currentTranscript);
                //currentTranscript.PrintDictionary();

                return currentTranscript;
            }
        }
    }
}
