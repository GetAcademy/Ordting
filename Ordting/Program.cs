using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;

namespace Ordting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var words = GetWords();
            //words.Where(w=>w.Length>=6&&w.Length<=10)
            var words7to10 = FilterOnWordLength(words, 7, 10);
            var random = new Random();
            var count = 0;
            for (var i = 0; i < 20; i++)
            {
                var word1 = GetRandomWord(random, words7to10);
                var word2 = GetMatchingWord(word1, 5, words7to10, words);
                string ending = "<ingen treff>";
                if (word2 != null) ending = word2.Substring(0, 5);
                else
                {
                    word2 = GetMatchingWord(word1, 4, words7to10, words);
                    if (word2 != null) ending = word2.Substring(0, 4);
                    else
                    {
                        word2 = GetMatchingWord(word1, 3, words7to10, words);
                        if (word2 != null) ending = word2.Substring(0, 3);
                    }
                }

                if (word2 == null) continue;
                count++;
                Console.WriteLine(count.ToString().PadLeft(2) + ": " + word1 + " " + word2 + " " + ending);
                //Console.ReadLine();
            }
        }

        private static string GetRandomWord(Random random, string[] words7to10)
        {
            var index = random.Next(0, words7to10.Length);
            var word1 = words7to10[index];
            return word1;
        }

        private static string GetMatchingWord(string word1,
            int commonLength, string[] words, string[] allWords)
        {
            var ending = word1.Substring(word1.Length - commonLength, commonLength);
            if (!allWords.Contains(ending)) return null;

            foreach (var word in words)
            {
                if (word.StartsWith(ending))
                {
                    return word;
                }
            }
            return null;
        }

        static string[] FilterOnWordLength(string[] words, int min, int max)
        {
            var list = new List<string>();
            foreach (var word in words)
                if (word.Length >= min && word.Length <= max)
                    list.Add(word);
            return list.ToArray();
        }

        static string[] GetWords()
        {
            var list = new List<string>();
            using (var reader = new StreamReader("Resources/fullform_bm.txt", Encoding.UTF8))
            {
                for (int i = 0; i < 30; i++) reader.ReadLine();
                string line;
                string lastWord = null;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split('\t');
                    var word = parts[1];
                    if (word == lastWord
                        || word.Contains("-")
                        || word.Contains(" ")
                        || char.IsUpper(word[0])
                        ) continue;
                    lastWord = word;
                    list.Add(word);
                }
            }

            return list.ToArray();
        }
    }
}
