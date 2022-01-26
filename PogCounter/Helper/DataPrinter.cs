using PogCounter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PogCounter.Helper
{
    class DataPrinter
    {
        public void PrintSingleTranscript(Transcript currentTranscript) 
        {
            string outputFile = $"../../../Results/{currentTranscript.GetTextFileName().Split(".")[0]}_analysis.md";

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
            PrintTranscript(currentTranscript);

            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
        }

        public void PrintTranscript(Transcript currentTranscript) 
        {


           // Console.WriteLine($"-----Analyzing {currentTranscript.GetVidTitle()}-----");
            


            currentTranscript.PrintMetadata();
            currentTranscript.PrintDictionary();

            Console.WriteLine();
            Console.WriteLine();
           // Console.WriteLine("Analysis completed.");

           // Console.WriteLine("--------------------------------------------------------------------------------");

        }
    }
}
