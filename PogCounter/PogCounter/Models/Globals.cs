using System;
using System.Collections.Generic;
using System.Text;

namespace PogCounter.Models
{
    static class Globals
    {
        public static char[] DelimiterCharacters = { ' ', ',', '.', ':', '?', '!', '-' };

        public static char[] OriginalPog = { 'p', 'o', 'g' };

        public static char[] LoveCount = { 'l', 'o', 'v', 'e' };

        public static List<string> Outliers = new List<string> { "parking", "pok", "pug", "plug", "pong" };

    }
}
