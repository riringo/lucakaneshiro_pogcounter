using PogCounter.Helper;
using PogCounter.Models;
using System;
using System.IO;
using System.Reflection;

namespace PogCounter
{
    class Program
    {
        public static Analyzer analyzer = new Analyzer();

        static void Main(string[] args)
        {

            //Check that path exists
            string TranscriptsPath = "../../../Transcripts";
            string outputFile = GetOutputFileName();

            Console.WriteLine("Welcome to the Word Counter. For full instructions on how to use this program, please refer to the ReadMe.");
            Console.WriteLine($"Reading files from {Path.GetFullPath(TranscriptsPath)} & outputting results to {Path.GetFullPath(outputFile)}");

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
                Console.WriteLine("Cannot open PogResults.txt for writing");
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(writer);

            var files = GetTranscripts(TranscriptsPath);
            foreach(var file in files) 
            {
                AnalyzeTranscript(file);
            }

            Console.WriteLine($"--------TOTALS of {files.Length} Transcripts--------");
            analyzer.PrintDictionary();
            analyzer.GetTotal();

            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
            Console.WriteLine("Analysis completed.");

        }

        public static string GetOutputFileName() 
        {
            string fileName = "PogResults";
            int duplicativeIndex = 1;
            string outputFile = $"../../../{fileName}.txt";

            while (File.Exists(outputFile))
            {
                outputFile = $"../../../{fileName}{duplicativeIndex}.txt";
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
            var currentTranscript = new Transcript(filePath);
            var fileName = filePath.Split("\\");
            Console.WriteLine($"-----Analyzing {fileName[fileName.Length - 1]}-----");
            var pogDictionary = analyzer.AnalyzeText(currentTranscript);
            currentTranscript.SetPogDictionary(pogDictionary);
            currentTranscript.PrintDictionary();
            Console.WriteLine("--------------------------------------------------------------------------------");
        }
    }
}
