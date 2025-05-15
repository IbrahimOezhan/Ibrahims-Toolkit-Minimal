using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TemplateTools
{
    public class Template_Utilities
    {
        // Miscellaneous
        public static List<T> ShuffleList<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(i, list.Count);
                (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
            }
            return list;
        }

        public static void Screenshot()
        {
            string fileName = "Screenshot-" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".png";
            string screenshotsPath = Path.Combine(Path_Utilities.GetGamePath(), "Screenshots");
            if (!Directory.Exists(screenshotsPath)) Directory.CreateDirectory(screenshotsPath);
            ScreenCapture.CaptureScreenshot(Path.Combine(screenshotsPath, fileName));
        }

        public static string[,] GetTable(string _text)
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
