#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class DropdownUpdater : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {

        }
    }
}

#endif