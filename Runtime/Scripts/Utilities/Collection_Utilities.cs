using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IbrahKit
{
    public static class Collection_Utilities
    {
        public static List<T> Shuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(i, list.Count);
                (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
            }
            return list;
        }

        public static T[] Shuffle<T>(T[] list)
        {
            T[] result = new T[list.Length];

            for (int i = 0; i < list.Length; i++)
            {
                result[i] = list[i];
            }

            for (int i = 0; i < result.Length; i++)
            {
                int randomIndex = UnityEngine.Random.Range(i, result.Length);
                (result[randomIndex], result[i]) = (result[i], result[randomIndex]);
            }

            return result;
        }

        public static T GetClampedArrayElement<T>(T[] array, int index)
        {
            if (array.Length == 0) return default;
            return array[Mathf.Clamp(index, 0, array.Length - 1)];
        }

        public static string[,] GetAsMatrix(string _text)
        {
            List<string> lineSplit = _text.Split('\n').ToList();

            lineSplit.RemoveAll(x => String_Utilities.IsEmpty(x.Trim()) || x.Trim() == "");

            int rowAmount = lineSplit.Count;
            int columnAmount = lineSplit[0].Split(';').Length;

            string[,] table = new string[columnAmount, rowAmount];

            for (int x = 0; x < columnAmount; x++)
            {
                for (int y = 0; y < rowAmount; y++)
                {
                    string[] rowSplit = lineSplit[y].Split(';');
                    table[x, y] = rowSplit[x];
                }
            }

            return table;
        }
    }
}
