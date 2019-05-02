using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ExtensionMethods;

namespace NeuralNetworkSummarization
{
    class Program
    {
        static List<double> ExtractFeature1(IEnumerable<String> noWhiteSpace, string Title)
        {
            string filePath = "Assets/stopwords-english.txt";
            var text = File.ReadAllLines(filePath);
            var F1 = new List<double>( );
            var trailingCharacters = new char[] { ',', '"', '\'' };

            foreach (var item in noWhiteSpace)
            {
                double f1Value = 0;
                var words = item.Split(' ');
                var clean = new List<string>( );

                foreach (var word in words)
                {
                    if (!text.Contains(word))
                    {
                        clean.Add(word);
                    }
                }

                var ltitle = Title.Split(' ');
                foreach (var word in clean)
                {
                    foreach (var trailing in trailingCharacters)
                    {
                        var rm = word.IndexOf(trailing);
                        if (rm != -1)
                            word.Remove(rm, 1);
                    }
                }
                foreach (var word in ltitle)
                {
                    if (clean.Contains(word)) { f1Value++; }
                }
                double relative = f1Value / clean.Count;
                F1.Add(relative);
            }

            // foreach (var item in F1)
            // {
            //     Console.WriteLine(item);
            // }
            return F1;
        }

        static List<int> ExtractFeature2(List<string> cleanedUpSentences)
        {
            int count = 0;
            var F2 = new List<int>( );
            foreach (var word in cleanedUpSentences.First( ).Split(' '))
            {
                foreach (var word2 in cleanedUpSentences[1].Split(' '))
                {
                    if (word == word2) count++;
                }
            }
            F2.Add(count);
            count = 0;
            for (int i = 1; i < cleanedUpSentences.Count - 1; i++)
            {
                int contar = 0;
                foreach (var item in cleanedUpSentences[i].Split(' '))
                {
                    foreach (var item2 in cleanedUpSentences[i - 1].Split(' '))
                    {
                        if (item == item2) contar++;

                    }
                    foreach (var item2 in cleanedUpSentences[i + 1].Split(' '))
                    {
                        if (item == item2) contar++;

                    }
                }

                F2.Add(contar);
            }

            foreach (var lastWord in cleanedUpSentences.Last( ).Split(' '))
            {
                foreach (var word2 in cleanedUpSentences[cleanedUpSentences.Count - 2].Split(' '))
                {
                    if (lastWord == word2) count++;
                }

            }
            F2.Add(count);
            return F2;
        }

        static int ExtractFeature3(IEnumerable<string> wordsInText, string sentence)
        {
            var counter = 0;
            var uniqueWords = new HashSet<string>( );
            foreach (var word in sentence.Split(' '))
            {
                if (!uniqueWords.Contains(word))
                {
                    uniqueWords.Add(word);
                }
            }

            foreach (var unique in uniqueWords)
            {
                counter += wordsInText.Count(word => word.ContainsWord(unique));
            }

            return counter;
        }

        // Feature 4: Contains special chars in a sentence
        static int ExtractFeature4(string sentence)
        {
            var specialCharacters = new string[] { "(", ")", "%", "$", "€", "£", "?", "!", "*", ":", ";" };
            int counter = 0;

            foreach (var special in specialCharacters)
            {
                if (sentence.Contains(special))
                {
                    ++counter;
                }
            }

            return counter;
        }

        static bool ExtractFeature5(string sentence)
        {
            var pattern = @"([a-zA-Z]+) (\d+)";
            Match result = Regex.Match(sentence, pattern);

            return result.Success;
        }

        static double ExtractFeature6(string sentence)
        {
            var howManyDigits = sentence.Count(c => char.IsNumber(c));
            return (double) howManyDigits / sentence.Length;
        }

        static double ExtractFeature7(IEnumerable<string> text, string sentence)
        {
            var numSentences = text.Count( );
            var average = text.Sum(s => s.Length) / numSentences;

            return (double) sentence.Length / average;
        }

        static IEnumerable<bool> IsInSummary(IEnumerable<string> originalText, IEnumerable<string> summarySentences)
        {
            var result = new List<bool>( );
            var summary = summarySentences.Where(s => s != string.Empty);

            foreach (var sentence in originalText)
            {
                result.Add(summary.Contains(sentence));
            }

            return result;
        }
        static void Main(string[] args)
        {
            string filePath = "Articles/001.txt";
            var text = File.ReadAllText(filePath).ToLower( );

            string summaryPath = "Summaries/001.txt";
            var summary = File.ReadAllText(summaryPath).ToLower( ).Split('.');

            // 1. Trim characters from leading or trailing sentences
            // 2. Split the text as sentences, based on periods or line-jumps
            // 3 Skip title and get all non-empty lines
            // 4. Trim all trailing and leading whitespace from each sentence
            // 5. Get title from article
            var trimChars = new [] { ',', '"', '\'', ' ' };
            var sentences = text.Split(new char[] { '.', '\n' });
            var notEmpty = sentences.Skip(1).Where(s => s != string.Empty);
            var sentenceNoWhiteSpace = notEmpty.Select(s => s.Trim(trimChars));
            var title = sentences.First( );

            // Clean up Text
            string stopWordsPath = "Assets/stopwords-english.txt";
            var stopWords = File.ReadAllText(stopWordsPath);

            // Cleaned up sentences, doesn't include stopwords or special characters
            var cleanedUpSentences = new List<string>( );
            foreach (var sentence in sentenceNoWhiteSpace)
            {
                var tempSentence = new List<string>( );
                var str = sentence.Split(' ').Select(s => s.Trim(trimChars));
                foreach (var word in str)
                {
                    var contains = stopWords.ContainsWord(word);
                    if (!contains) tempSentence.Add(word);
                }
                cleanedUpSentences.Add(string.Join(' ', tempSentence));
            }

            // Break up the text into words and remove stopwords and special characters
            var cleanedWordsInText = cleanedUpSentences.SelectMany(s => s.Split(' '));

            Console.WriteLine( );
            var F1 = ExtractFeature1(sentenceNoWhiteSpace, title);
            var F2 = ExtractFeature2(cleanedUpSentences);
            var F3 = new List<int>( );
            foreach (var sentence in cleanedUpSentences)
            {
                // This has to receive all words in the text
                F3.Add(ExtractFeature3(cleanedWordsInText, sentence));
            }

            var F4 = new List<int>( );
            foreach (var sentence in cleanedUpSentences)
            {
                F4.Add(ExtractFeature4(sentence));
            }

            var F5 = new List<bool>( );
            foreach (var sentence in cleanedUpSentences)
            {
                F5.Add(ExtractFeature5(sentence));
            }

            var F6 = new List<double>( );
            foreach (var sentence in cleanedUpSentences)
            {
                F6.Add(ExtractFeature6(sentence));
            }

            var F7 = new List<double>( );
            foreach (var sentence in cleanedUpSentences)
            {
                F7.Add(ExtractFeature7(cleanedUpSentences, sentence));
            }

            var Resultados = IsInSummary(sentenceNoWhiteSpace, summary).ToList( );

            var lines = new List<string>( );

            var name = "@RELATION article\n";
            var F1N = "@ATTRIBUTE F1 REAL";
            var F2N = "@ATTRIBUTE F2 REAL";
            var F3N = "@ATTRIBUTE F3 REAL";
            var F4N = "@ATTRIBUTE F4 REAL";
            var F5N = "@ATTRIBUTE F5 {True,False}";
            var F6N = "@ATTRIBUTE F6 REAL";
            var F7N = "@ATTRIBUTE F7 REAL";
            var RESULT = "@ATTRIBUTE RESULT {True,False}\n";
            var DATA = "@DATA";

            lines.Add(name);
            lines.Add(F1N);
            lines.Add(F2N);
            lines.Add(F3N);
            lines.Add(F4N);
            lines.Add(F5N);
            lines.Add(F6N);
            lines.Add(F7N);
            lines.Add(RESULT);
            lines.Add(DATA);

            for (int i = 0; i < F1.Count; i++)
            {
                lines.Add($"{F1[i]},{F2[i]},{F3[i]},{F4[i]},{F5[i]},{F6[i]},{F7[i]},{Resultados[i]}");
            }

            File.WriteAllLines("result.arff", lines);
            Console.WriteLine( );

        }
    }
}