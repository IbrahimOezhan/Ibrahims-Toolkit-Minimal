using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace TemplateTools
{
    /// <summary>
    /// A script that manages loading data on game start and saving it when you close the game
    /// </summary>
    [DefaultExecutionOrder(-5)]
    public partial class Save_Manager : Manager_Base
    {
        private const string encryptionKey = "a3c9e7r3gf3d5e7";

        public static Save_Manager Instance;
        public static SaveFolder currentFolder;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else
            {
                Instance = this;

                string saveFolderPath = Path.Combine(Path_Utilities.GetGamePath(), "Saves");
                string currentSaveFolder = Path.Combine(saveFolderPath, "Current");

                SaveFolder saveFolder = new(currentSaveFolder, encryptionKey);

                List<string> allDirectories = Directory.GetDirectories(saveFolderPath).ToList();

                allDirectories.Remove(currentSaveFolder);

                if (saveFolder.ValidateSaves())
                {
                    Debug.LogWarning("Current save folder failed validation");

                    allDirectories.Sort((a, b) =>
                    {
                        return String_Utilities.CompareVersions(Path.GetDirectoryName(a), Path.GetDirectoryName(b));
                    });

                    foreach (string directory in allDirectories)
                    {
                        SaveFolder folder = new(directory, encryptionKey);
                        if (!folder.ValidateSaves())
                        {
                            Debug.LogWarning("Folder with version " + folder.GetVersion() + " succeded validation");
                            currentFolder = folder;
                            break;
                        }
                    }

                    Directory.Move(currentSaveFolder, Path.Combine(saveFolderPath, saveFolder.GetVersion()));

                    saveFolder = new(currentSaveFolder, encryptionKey);

                    Debug.LogWarning("Migrating old save to version folder");
                }

                currentFolder = saveFolder;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                currentFolder.SaveGenericData();
            }
        }
    }
}