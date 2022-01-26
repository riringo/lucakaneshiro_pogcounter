using PogCounter.Helper;
using PogCounter.Models;
using System;
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


            var files = GetTranscripts(TranscriptsPath);
            foreach (var file in files)
            {
                AnalyzeTranscript(file);
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

            Console.WriteLine($"## TOTALS of {files.Length} Transcripts");
            analyzer.PrintDictionary();
            Console.WriteLine();
            analyzer.GetTotal();
            Console.WriteLine();
            analyzer.PrintLoveDictionary();
            Console.WriteLine();
            analyzer.GetLoveTotal();

            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
            Console.WriteLine("Analysis completed.");

        }


        public static string GetOutputFileName()
        {
            string fileName = "PogResults";
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

        public static void AnalyzeTranscript(string filePath)
        {
            var fileParts = filePath.Split("\\");
            var fileName = fileParts[fileParts.Length - 1];

            var rowData = metadataTable.Select($"textFile = '{fileName}'")[0];
            if (rowData == null)
            {
                Console.WriteLine($"Cannot find {fileName} in the Metadata information. Please update LucaKaneshiroStreams.csv file.");
            }
            else
            {

                var currentTranscript = new Transcript(filePath, rowData);
                var pogDictionary = analyzer.AnalyzeText(currentTranscript);
                currentTranscript.SetPogDictionary(pogDictionary);
                resultsPrinter.PrintTranscript(currentTranscript);
                //currentTranscript.PrintDictionary();
            }
        }
    }
}
