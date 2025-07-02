using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace HelloWorld.Utils
{
    public static class BadWordsFiter
    {
        private static HashSet<string> badWords;

        private static readonly Dictionary<char, char> leetReplacements = new()
        {
            { '0', 'o' }, { '1', 'i' }, { '3', 'e' }, { '4', 'a' }, { '5', 's' },
            { '7', 't' }, { '@', 'a' }, { '$', 's' }, { '!', 'i' }, { '8', 'b' },
            { '9', 'g' }, { '6', 'g' }, { '|', 'l' }, { '+', 't' }, { '€', 'e' },
            { '£', 'l' }, { '¢', 'c' }, { '§', 's' }, { '?', 'q' }
        };

        public static void LoadAll()
        {
            if (badWords != null) return;

            badWords = new HashSet<string>();

            TextAsset[] textFiles = Resources.LoadAll<TextAsset>("BadWords");
            foreach (TextAsset file in textFiles)
            {
                string[] lines = file.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    badWords.Add(line.Trim().ToLower());
                }
            }

            Debug.Log($"[BadWordsFilter] {badWords.Count} palavras carregadas de {textFiles.Length} arquivos.");
        }

        public static bool ContainsBadWord(string input)
        {
            if (badWords == null)
                LoadAll();

            string[] words = input.Split(new[] { ' ', '.', ',', '-', '_', '\n', '\r', '!', '?', ';', ':' }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                string cleaned = Normalize(word);
                if (badWords.Contains(cleaned))
                    return true;
            }

            return false;
        }


        public static string Censor(string input)
        {
            if (badWords == null)
                LoadAll();

            string[] words = input.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                string cleaned = Normalize(words[i]);
                if (badWords.Contains(cleaned))
                    words[i] = new string('*', words[i].Length);
            }

            return string.Join(" ", words);
        }



        private static string Normalize(string word)
        {
            word = word.ToLower();

            // Remove acentos
            string normalized = word.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char ch in normalized)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    char replaced = leetReplacements.ContainsKey(ch) ? leetReplacements[ch] : ch;
                    if (char.IsLetterOrDigit(replaced)) // remove pontuações, emojis, etc.
                        sb.Append(replaced);
                }
            }

            return sb.ToString();
        }
    }
}
