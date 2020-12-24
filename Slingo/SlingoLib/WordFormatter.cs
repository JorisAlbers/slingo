using System;
using System.Collections.Generic;
using System.Text;

namespace SlingoLib
{
    public static class WordFormatter
    {
        public static string Format(string word)
        {
            word = word.ToLower();
            return word.Replace("ij", "ĳ"); // digraph 'ij' to monograph ij
        }
    }

}
