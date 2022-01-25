using PogCounter.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PogCounter.Helper
{
    class Analyzer
    {
        private Dictionary<string, int> allPogInstances;

        public Analyzer() 
        {
            this.allPogInstances = new Dictionary<string, int>();
        }

        public Dictionary<string, int> AnalyzeText(Transcript currentTranscipt) 
        {
            var pogDictionary = new Dictionary<string, int>();

            var totalWords = currentTranscipt.GetWordCount();
            var wordIndex = 0;
            var wordsList = currentTranscipt.GetAllWords().ToArray();
            while (wordIndex < totalWords)
            {
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
                            while (pogCharIndex < 2 && isPog && charIndex < word.Length)
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
                            if (pogCharIndex < 2)
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

            Console.WriteLine("Dictionary: ");
            foreach (var key in this.allPogInstances.Keys)
            {
                Console.WriteLine($"\t{key}: {this.allPogInstances[key]}");
            }
        }
        public void GetTotal()
        {
            var countPogs = 0;
            foreach (var key in this.allPogInstances.Keys)
            {
                countPogs += this.allPogInstances[key];
            }
            Console.WriteLine($"Total Count: {countPogs}");
        }

    }
}
