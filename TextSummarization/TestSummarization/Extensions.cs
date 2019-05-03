using System;
using System.Linq;

namespace ExtensionMethods
{
    public static class Extensions
    {
        public static bool ContainsWord(this string sentence, string searched)
        {
            var trim = new char[] { ',', '"', '\'' };
            var words = sentence.Split('\n').Select(w => w.Trim(trim).Replace("\r", ""));
            foreach (var word in words)
            {
                if(string.Equals(word, searched)) return true;
            }

            return false;
        }
    }
}