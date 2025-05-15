using System;
using System.Diagnostics;
using System.IO;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Path_Utilities
{
    private const string myGames = "My Games";

#if UNITY_EDITOR
    [MenuItem("Template/OpenPath")]
    public static void OpenPath()
    {
        Process.Start(GetGamePath());
    }
#endif

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