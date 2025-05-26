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

                Debug_Manager.bufferLogs = true;

                string saveFolderPath = Path.Combine(Path_Utilities.GetGamePath(), "Saves");
                string currentSaveFolder = Path.Combine(saveFolderPath, "Current");

                SaveFolder saveFolder = new(currentSaveFolder, encryptionKey);

                List<string> allDirectories = Directory.GetDirectories(saveFolderPath).ToList();

                allDirectories.Remove(currentSaveFolder);

                // Enters statement if save folder is not compatible anymore
                if (saveFolder.ValidateSaves())
                {
                    int versionCompare = String_Utilities.CompareVersions(saveFolder.GetVersion(), Application.version);

                    // Current is newer version than the save files version
                    if (versionCompare > 0)
                    {
                        string oldVersionPath = Path.Combine(saveFolderPath, saveFolder.GetVersion());

                        //Backup data to new folder named the old version
                        Directory.CreateDirectory(oldVersionPath);
                        SaveFolder.CopyAll(saveFolder, oldVersionPath);
                    }
                    // Current is older meaning an older version of the game was launched after a newer one was already launched
                    else if(versionCompare < 0)
                    {
                        //Sort list by version number
                        allDirectories.Sort((a, b) =>
                        {
                            return String_Utilities.CompareVersions(Path.GetDirectoryName(a), Path.GetDirectoryName(b));
                        });
                    }

                    Debug.LogWarning("Current save folder failed validation");

                    

                    // Check if old version is compatible
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


                    saveFolder = new(currentSaveFolder, encryptionKey);

                    Debug.LogWarning("Migrating old save to version folder");
                }

                currentFolder = saveFolder;

                Debug.ReleaseBuffer();
                Debug_Manager.bufferLogs = false;
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