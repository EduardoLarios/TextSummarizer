﻿using System;
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
            var F1 = new List<double>();
            var trailingCharacters = new char[] { ',', '"', '\'' };

            foreach (var item in noWhiteSpace)
            {
                double f1Value = 0;
                var words = item.Split(' ');
                var clean = new List<string>();

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

            return F1;
        }

        static List<int> ExtractFeature2(List<string> cleanedUpSentences)
        {
            int count = 0;
            var F2 = new List<int>();
            foreach (var word in cleanedUpSentences.First().Split(' '))
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

            foreach (var lastWord in cleanedUpSentences.Last().Split(' '))
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
            var uniqueWords = new HashSet<string>();
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

        static int ExtractFeature6(string sentence, IEnumerable<string> top50Words)
        {
            var count = 0;
            var words = sentence.Split(' ');
            foreach (var word in words)
            {
                foreach (var important in top50Words)
                {
                    if (word.Equals(important)) count++;
                }
            }

            return count;
        }

        static double ExtractFeature7(IEnumerable<string> text, string sentence)
        {
            var numSentences = text.Count();
            var average = text.Sum(s => s.Length) / numSentences;

            return (double) sentence.Length / average;
        }

        static IEnumerable<bool> IsInSummary(IEnumerable<string> originalText, IEnumerable<string> summarySentences)
        {
            var result = new List<bool>();
            var summary = summarySentences.Where(s => s != string.Empty);

            foreach (var sentence in originalText)
            {
                result.Add(summary.Contains(sentence));
            }

            return result;
        }
        static void Main()
        {
            // Load all articles and all summaries
            var allArticles = Directory.GetFiles("Articles");
            var allSummaries = Directory.GetFiles("Summaries");

            // Load stopwords to clean up the article
            string stopWordsPath = "Assets/stopwords-english.txt";
            var stopWords = File.ReadAllText(stopWordsPath);
            var current = 0;

            // Create result folders
            Directory.CreateDirectory("TrainingData");
            Directory.CreateDirectory("TestData");

            var allArticleWords = new List<string>();
            Console.WriteLine("Starting...");
            Console.WriteLine($"Processing {allArticles.Length} articles");

            Console.WriteLine("Choose the type of article.");
            Console.WriteLine("1 - Bussiness");
            Console.WriteLine("2 - Entertainment");
            Console.WriteLine("3 - Politics");
            Console.WriteLine("4 - Sport");
            Console.WriteLine("5 - Tech");
            // Gets all words from all articles
            
            Int32.TryParse(Console.ReadLine(), out int topic);
            System.Console.WriteLine("Select One of these Files");

            var select = "";
            var article = "";
            var TopicList = new List<string>();
            var model = "";
            var classifiers = "";
            switch (topic)
            {
                case 1:
                    TopicList = Directory.GetFiles("Articles/business").ToList();
                    
                    foreach (var item in TopicList)
                    {
                        System.Console.WriteLine(item);    
                    }
                    select = Console.ReadLine();
                    article = $"Articles/business/{select}";
                    model = "business.model";
                    classifiers = "functions.MultilayerPerceptron";
                    break;
                case 2:
                    TopicList = Directory.GetFiles("Articles/entertainment").ToList();
                    
                    foreach (var item in TopicList)
                    {
                        System.Console.WriteLine(item);    
                    }
                    select = Console.ReadLine();
                    article = $"Articles/entertainment/{select}";
                    model = "entertainment.model"; 
                    classifiers = "functions.MultilayerPerceptron";               
                    break;
                case 3:
                    TopicList = Directory.GetFiles("Articles/politics").ToList();
                    
                    foreach (var item in TopicList)
                    {
                        System.Console.WriteLine(item);    
                    }
                    select = Console.ReadLine();
                    article = $"Articles/politics/{select}"; 
                    model = "politics.model"; 
                    classifiers = "functions.MultilayerPerceptron";
                    break;
                case 4:
                    TopicList = Directory.GetFiles("Articles/sport").ToList();
                    
                    foreach (var item in TopicList)
                    {
                        System.Console.WriteLine(item);    
                    }
                    select = Console.ReadLine();
                    article = $"Articles/sport/{select}"; 
                    model = "sport.model";
                    classifiers = "functions.MultilayerPerceptron"; 
                    break;
                case 5:
                    TopicList = Directory.GetFiles("Articles/tech").ToList();
                    
                    foreach (var item in TopicList)
                    {
                        System.Console.WriteLine(Path.GetFileName(item));    
                    }
                    select = Console.ReadLine();
                    article = $"Articles/tech/{select}";
                    model = "tech.model";
                    classifiers = "meta.RandomCommittee";
                    break;
            }
            
            
            Console.WriteLine("Getting important words...");
            {
                var text = File.ReadAllText(article).ToLower();
                var trimChars = new [] { ',', '"', '\'', ' ' };
                var sentences = text.Split(new char[] { '.', '\n' });
                var notEmpty = sentences.Skip(1).Where(s => s != string.Empty);
                var sentenceNoWhiteSpace = notEmpty.Select(s => s.Trim(trimChars));

                // Cleaned up sentences, doesn't include stopwords or special characters
                var cleanedUpSentences = new List<string>();
                foreach (var sentence in sentenceNoWhiteSpace)
                {
                    var tempSentence = new List<string>();
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
                cleanedWordsInText = cleanedWordsInText.Where(word => word != "-" && word != "");
                allArticleWords.AddRange(cleanedWordsInText);
            }

            var importantWords = new Dictionary<string, int>();
            foreach (var word in allArticleWords)
            {
                if (!importantWords.ContainsKey(word))
                {
                    importantWords.Add(word, 1);
                }
                else
                {
                    ++importantWords[word];
                }
            }

            // Gets all top 50 important words per topic
            var orderedImportantWords = from entry in importantWords orderby entry.Value descending select entry;
            var top50Words = orderedImportantWords.Take(50).Select(e => e.Key);

            Console.WriteLine("Generating datasets...");
            // Generates features for ARFF dataset
            
            {
                var text = File.ReadAllText(article).ToLower();
                var fileName = Path.GetFileName(article);

                string summaryPath = allSummaries[current];
                var summary = File.ReadAllText(summaryPath).ToLower().Split('.');

                // 1. Trim characters from leading or trailing sentences
                // 2. Split the text as sentences, based on periods or line-jumps
                // 3 Skip title and get all non-empty lines
                // 4. Trim all trailing and leading whitespace from each sentence
                // 5. Get title from article
                var trimChars = new [] { ',', '"', '\'', ' ' };
                var sentences = text.Split(new char[] { '.', '\n' });
                var notEmpty = sentences.Skip(1).Where(s => s != string.Empty);
                var sentenceNoWhiteSpace = notEmpty.Select(s => s.Trim(trimChars));
                var title = sentences.First();

                // Cleaned up sentences, doesn't include stopwords or special characters
                var cleanedUpSentences = new List<string>();
                foreach (var sentence in sentenceNoWhiteSpace)
                {
                    var tempSentence = new List<string>();
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
                var F1 = ExtractFeature1(sentenceNoWhiteSpace, title);
                var F2 = ExtractFeature2(cleanedUpSentences);
                var F3 = new List<int>();
                foreach (var sentence in cleanedUpSentences)
                {
                    // This has to receive all words in the text
                    F3.Add(ExtractFeature3(cleanedWordsInText, sentence));
                }

                var F4 = new List<int>();
                foreach (var sentence in cleanedUpSentences)
                {
                    F4.Add(ExtractFeature4(sentence));
                }

                var F5 = new List<bool>();
                foreach (var sentence in cleanedUpSentences)
                {
                    F5.Add(ExtractFeature5(sentence));
                }

                var F6 = new List<double>();
                foreach (var sentence in cleanedUpSentences)
                {
                    F6.Add(ExtractFeature6(sentence, top50Words));
                }

                var F7 = new List<double>();
                foreach (var sentence in cleanedUpSentences)
                {
                    F7.Add(ExtractFeature7(cleanedUpSentences, sentence));
                }

                var Resultados = IsInSummary(sentenceNoWhiteSpace, summary).ToList();

                var training = new List<string>();
                var testing = new List<string>();
                var header = 
            @"@RELATION article

@ATTRIBUTE F1 REAL
@ATTRIBUTE F2 REAL
@ATTRIBUTE F3 REAL
@ATTRIBUTE F4 REAL
@ATTRIBUTE F5 {True,False}
@ATTRIBUTE F6 REAL
@ATTRIBUTE F7 REAL
@ATTRIBUTE RESULT {True,False}

@DATA";
                testing.Add(header);
                for (int i = 0; i < F1.Count; i++)
                {
                    training.Add($"{F1[i]},{F2[i]},{F3[i]},{F4[i]},{F5[i]},{F6[i]},{F7[i]},{Resultados[i]}");
                    testing.Add($"{F1[i]},{F2[i]},{F3[i]},{F4[i]},{F5[i]},{F6[i]},{F7[i]},?");
                }

                File.WriteAllLines($"real_{current}.arff", training);
                File.WriteAllLines($"testing.arff", testing);
                ++current;
                Console.WriteLine("Finished processing all files");

                //EstoDeberia estar en otro lado
                // foreach (var item in sentenceNoWhiteSpace)
                // {
                //     System.Console.WriteLine(item);
                // }
                System.Console.WriteLine("Click Enter To Continue");
                var output = $"java weka.classifiers.{classifiers} -l {model} -p 0 -T testing.arff > prediction.txt".Bash();

                var nothing = Console.ReadLine();
                var pred = File.ReadAllText("prediction.txt");
                var sentencespred = pred.Split('\n');
                var ListSentenceNoWhiteSpace = sentenceNoWhiteSpace.ToList();
                var OutputPrediction = new List<string>();
               
                for (int i = 5; i < sentencespred.Length; i++)
                {
                    
                    if(sentencespred[i].Contains("True"))
                    {
                        System.Console.WriteLine("Sentence Number:  "+i);
                        System.Console.WriteLine(ListSentenceNoWhiteSpace[i-5]);
                        OutputPrediction.Add(ListSentenceNoWhiteSpace[i-5]+".");
                    }
                }
                File.WriteAllLines($"outputPrediction.txt", OutputPrediction);                   
            }
        }
    }
}