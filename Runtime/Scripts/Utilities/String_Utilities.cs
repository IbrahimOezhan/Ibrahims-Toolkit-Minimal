using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IbrahKit
{
    public class String_Utilities
    {
        private static readonly string[] zalgoChars =
        {
            "\u0300", "\u0301", "\u0302", "\u0303", "\u0304", "\u0305", "\u0306", "\u0307", "\u0308",
            "\u0309", "\u030A", "\u030B", "\u030C", "\u030D", "\u030E", "\u030F", "\u0310", "\u0311",
            "\u0312", "\u0313", "\u0314", "\u0315", "\u0316", "\u0317", "\u0318", "\u0319", "\u031A",
            "\u031B", "\u031C", "\u031D", "\u031E", "\u031F", "\u0320", "\u0321", "\u0322", "\u0323",
            "\u0324", "\u0325", "\u0326", "\u0327", "\u0328", "\u0329", "\u032A", "\u032B", "\u032C",
            "\u032D", "\u032E", "\u032F", "\u0330", "\u0331", "\u0332", "\u0333", "\u0334", "\u0335",
            "\u0336", "\u0337", "\u0338", "\u0339", "\u033A", "\u033B", "\u033C", "\u033D", "\u033E",
            "\u033F", "\u0340", "\u0341", "\u0342", "\u0343", "\u0344", "\u0345", "\u0346", "\u0347",
            "\u0348", "\u0349", "\u034A", "\u034B", "\u034C", "\u034D", "\u034E", "\u034F"
        };

        public static string GenerateZalgoText(string _inputText, int _intensity)
        {
            StringBuilder _zalgoText = new();

            foreach (char _c in _inputText)
            {
                _zalgoText.Append(_c);
                for (int i = 0; i < _intensity; i++)
                {
                    int _numChars = Random.Range(1, 4);
                    for (int j = 0; j < _numChars; j++)
                    {
                        _zalgoText.Append(zalgoChars[Random.Range(0, zalgoChars.Length)]);
                    }
                }
            }

            return _zalgoText.ToString();
        }

        public static string GenerateZalgoText(char _inputChar, int _intensity)
        {
            StringBuilder _zalgoText = new();

            _zalgoText.Append(_inputChar);
            for (int i = 0; i < _intensity; i++)
            {
                int _numChars = Random.Range(1, 4);
                for (int j = 0; j < _numChars; j++)
                {
                    _zalgoText.Append(zalgoChars[Random.Range(0, zalgoChars.Length)]);
                }
            }
            return _zalgoText.ToString();
        }


        public static string StringListToString(List<string> list, char delimiter)
        {
            StringBuilder sb = new();
            for (int i = 0; i < list.Count; ++i)
            {
                sb.Append(list[i] + (i == list.Count - 1 ? "" : delimiter));
            }
            return sb.ToString();
        }

        public static string StringListToString(List<string> list)
        {
            string finalString = list.Aggregate(string.Empty, (current, next) => current + (current == string.Empty ? "" : ", ") + next);
            return finalString;
        }

        public static void CreateDropdown(List<string> input, string fileName)
        {
            if (input == null)
            {
                Debug.LogWarning("Passed input list is null");
                return;
            }

            if (input.Count == 0)
            {
                Debug.LogWarning("Passed input list is empty. Possible error");
                return;
            }

            if (IsEmpty(fileName))
            {
                Debug.LogWarning("File name is empty or null");
                return;
            }

            for (int i = input.Count - 1; i >= 0; i--)
            {
                if (IsEmpty(input[i]))
                {
                    Debug.LogWarning("List contains empty keys. Removing");
                    input.RemoveAt(i);
                }
            }

            List<string> distinct = input.Distinct().ToList();

            if (input.Count != distinct.Count)
            {
                Debug.LogError("Duplicate keys found in input");

                List<string> duplicates = input.Except(distinct).ToList();

                for (int i = 0; i < duplicates.Count; i++)
                {
                    Debug.LogError("Duplicate: " + duplicates[i]);
                }

                return;
            }

            string _dir = "Assets/Resources/DropdownFiles/";

            fileName += ".txt";

            string _filePath = _dir + fileName;

            if (!Directory.Exists(_dir)) Directory.CreateDirectory(_dir);

            using (StreamWriter sw = new StreamWriter(_filePath))
            {
                for (int i = 0; i < input.Count; i++)
                {
                    sw.WriteLine(input[i]);
                }
            }
        }

        public static List<string> GetDropdown(string filePath)
        {
            return File.ReadAllLines(filePath).ToList();

        }

        public static bool IsEmpty(string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value) || value == string.Empty || value.Equals(string.Empty);
        }

        public static List<List<string>> GetTable(string text, char delimiter)
        {
            List<List<string>> result = new();

            string[] lines = text.Split("\n");

            for (int i = 0; i < lines.Length; i++)
            {
                if (IsEmpty(lines[i])) continue;
                if (IsEmpty(lines[i].Replace(delimiter.ToString(), ""))) continue;

                string[] columns = lines[i].Split(delimiter);

                if (columns.Length == 0) continue;

                result.Add(new());

                for (int c = 0; c < columns.Length; c++)
                {
                    result[^1].Add(columns[c]);
                }
            }

            return result;
        }

        public static int CompareVersions(string v1, string v2)
        {
            string[] aSplit = v1.Split('.');
            string[] bSplit = v2.Split('.');

            int longestLength = Mathf.Max(aSplit.Length, bSplit.Length);

            for (int i = 0; i < longestLength; i++)
            {
                if (i >= aSplit.Length) return -1;
                if (i >= bSplit.Length) return 1;

                int numA = int.Parse(aSplit[i]);
                int numB = int.Parse(bSplit[i]);

                int dif = numA - numB;

                if (dif != 0) return dif;
            }

            return 0;
        }

        public static string DecryptEncrypt(string _data, string key)
        {
            string _result = "";
            for (int i = 0; i < _data.Length; i++) _result += (char)(_data[i] ^ key[i % key.Length]);
            return _result;
        }
    }
}