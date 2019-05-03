using System;
using System.IO;
using System.Linq;

namespace ExtractSummary
{
    class Program
    {
        static void Main(string[] args)
        {
            var article = "article.txt";
            var text = File.ReadAllText(article);
            var trimChars = new[] { ',', '"', '\'', ' ' };
            var sentences = text.Split(new char[] { '.', '\n' });
            var notEmpty = sentences.Skip(1).Where(s => s != string.Empty);
            var sentenceNoWhiteSpace = notEmpty.Select(s => s.Trim(trimChars));
            var title = sentences.First();
        }
    }
}
