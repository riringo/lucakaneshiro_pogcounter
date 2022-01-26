using PogCounter.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PogCounter.Helper
{
    class Analyzer
    {
        private Dictionary<string, int> allPogInstances;
        private Dictionary<string, int> allLoveInstances;
        private int totalWords;
        private int totalRunTime;

        public Analyzer() 
        {
            this.allPogInstances = new Dictionary<string, int>();
            this.allLoveInstances = new Dictionary<string, int>();
            this.totalWords = 0;
            this.totalRunTime = 0;
        }

        public Dictionary<string, int> AnalyzeText(Transcript currentTranscipt) 
        {
            var pogDictionary = new Dictionary<string, int>();
            this.totalRunTime += currentTranscipt.GetRunTimeMinutes();
            var totalWords = currentTranscipt.GetWordCount();
            var wordIndex = 0;
            var wordsList = currentTranscipt.GetAllWords().ToArray();
            while (wordIndex < totalWords)
            {
                this.totalWords++;
                var word = wordsList[wordIndex];
                if (word.Length > 1)
                {
                    if (word[0] == Globals.OriginalPog[0]) //Checks if word starts with "P"
                    {
                        if (word.Length > 1)
                        {
                            //CHECKS IF WORD IS POG, POOOG, or POGGGG
                            var pogCharIndex = 0;
                            var isPog = true;
                            var charIndex = 1;
                            while (pogCharIndex < Globals.OriginalPog.Length -1 && isPog && charIndex < word.Length)
                            {
                                var currentChar = word[charIndex];

                                if (currentChar != Globals.OriginalPog[pogCharIndex])
                                {
                                    if (currentChar == Globals.OriginalPog[pogCharIndex + 1])
                                    {
                                        pogCharIndex++;
                                    }
                                    else
                                    {
                                        isPog = false;

                                    }
                                }
                                charIndex++;

                            }
                            if (pogCharIndex < Globals.OriginalPog.Length - 1)
                            {
                                isPog = false;
                            }
                            if (isPog
                                || Globals.Outliers.Contains(word) //check for known outliers
                            )
                            {
                                if (pogDictionary.ContainsKey(word))
                                {
                                    var newValue = pogDictionary[word] + 1;
                                    pogDictionary[word] = newValue;
                                }
                                else
                                {
                                    pogDictionary[word] = 1;
                                }
                            }
                        }
                    } else if (word == "love" || word=="loves" || word=="loved")
                    {
                        if (this.allLoveInstances.ContainsKey(word))
                        {
                            var newValue = this.allLoveInstances[word] + 1;
                            this.allLoveInstances[word] = newValue;
                        }
                        else
                        {
                            this.allLoveInstances[word] = 1;
                        }
                    }


                }
                wordIndex++;
            }

            CombineWithGlobal(pogDictionary);
            return pogDictionary;
        }
        
        private void CombineWithGlobal(Dictionary<string, int> currentDict) 
        {
            foreach (var currentPogWord in currentDict.Keys)
            {
                if (this.allPogInstances.ContainsKey(currentPogWord))
                {
                    var oldValue = this.allPogInstances[currentPogWord];
                    this.allPogInstances[currentPogWord] = oldValue + currentDict[currentPogWord];
                }
                else
                {
                    this.allPogInstances[currentPogWord] = currentDict[currentPogWord];
                }
            }
        }
        public void PrintDictionary()
        {

            //Console.WriteLine("Dictionary: ");
            Console.WriteLine("Complete list of all the pogs found from all transcripts");
            Console.WriteLine();
            Console.WriteLine("#### Pog Dictionary");
            Console.WriteLine("**Pog-like Word** | **Count**");
            Console.WriteLine(":---: | :---:");
            foreach (var key in this.allPogInstances.Keys)
            {
                Console.WriteLine($"{key} | {this.allPogInstances[key]}");
            }
        }

        public void PrintLoveDictionary()
        {
            Console.WriteLine("### A bit of extra fun....LOVE INSTANCES!");
            Console.WriteLine("#### Love Dictionary");
            Console.WriteLine();
            Console.WriteLine("**Love-like Word** | **Count**");
            Console.WriteLine(":---: | :---:");
            foreach (var key in this.allLoveInstances.Keys)
            {
                Console.WriteLine($"{key} | {this.allLoveInstances[key]}");
            }
        }

        public void GetTotal()
        {
            var countPogs = 0;
            foreach (var key in this.allPogInstances.Keys)
            {
                countPogs += this.allPogInstances[key];
            }
            Console.WriteLine($"### Total Runtime: **{this.totalRunTime/60} hours & {this.totalRunTime%60} minutes**");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"### Total Words: **{this.totalWords} words**");
            Console.WriteLine($"### Total Count: **{countPogs} POGS**");
            Console.WriteLine($"### Pog Per Minute (# of Pogs / Runtime in minutes): {(double) countPogs / (double) this.totalRunTime }");
            Console.WriteLine();
            Console.WriteLine($"### Pog Density (# of Pogs / Total Words): {(double) countPogs / (double) this.totalWords}");
            Console.WriteLine();
        }

        public void GetLoveTotal()
        {
            var countLove = 0;
            foreach (var key in this.allLoveInstances.Keys)
            {
                countLove += this.allLoveInstances[key];
            }
            Console.WriteLine($"### Total Count: **{countLove} LOVES**");
        }
    }
}
