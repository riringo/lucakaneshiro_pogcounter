using System;
using System.Collections.Generic;
using System.Text;

namespace PogCounter.Models
{
    interface ITranscript
    {
        public List<string> GetAllWords();

        public bool IsFileValid();

        public int GetWordCount();

        public string GetFileSource();

        public void SetPogDictionary(Dictionary<string, int> keywordDict);

        public int GetPogCount();

        public double GetPogDensity();

        public void PrintDictionary();

        public string GetVidTitle();
        public string GetTextFileName();
        public int GetRunTimeMinutes();
        public string GetYTLink();
        public double GetPogPerMinute();
    }
}
