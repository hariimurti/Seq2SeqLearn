using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Seq2SeqLearn.Tools
{
    public static class StringManipulation
    {
        public static List<string> SplitWordToList(this string input)
        {
            // add space before dot, coma, etc
            input = Regex.Replace(input, @"([\.,;?!])", delegate (Match m)
            {
                return m.ToString().Replace(m.Groups[1].Value, " " + m.Groups[1].Value);
            });

            var words = new List<string>();
            var matches = Regex.Matches(input, @"(\S+)");
            foreach (Match m in matches)
            {
                words.Add(m.Groups[1].Value);
            }
            return words;
        }

        public static string JoinToSentence(this List<string> words)
        {
            var sentence = string.Join(" ", words);
            // remove space before dot, coma, etc
            sentence = Regex.Replace(sentence, @"( +)[\.,;?!]", delegate (Match m)
            {
                return m.ToString().Replace(m.Groups[1].Value, "");
            });
            return sentence;
        }

        public static bool AreEqual(this List<List<string>> listOne, List<List<string>> listTwo)
        {
            if (listOne.Count != listTwo.Count) return false;
            for (int i = 0; i < listOne.Count; i++)
            {
                if (!listOne[i].SequenceEqual(listTwo[i])) return false;
            }
            return true;
        }
    }
}
