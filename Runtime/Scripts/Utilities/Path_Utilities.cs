using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace IbrahKit
{
    public static class Path_Utilities
    {
        private const string myGames = "My Games";

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Template/OpenPath")]
#endif
        public static void OpenPath()
        {
            Process.Start(GetGamePath());
        }

        public static string GetGamePath()
        {
            string gamePath;

            if (!Application.isMobilePlatform)
            {
                gamePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), myGames);
            }
            else
            {
                gamePath = Application.persistentDataPath;
            }

            gamePath = Path.Combine(gamePath, Application.productName);

            if (!Directory.Exists(gamePath)) Directory.CreateDirectory(gamePath);

            return gamePath;
        }
    }
}