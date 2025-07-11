using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace IbrahKit
{
    /// <summary>
    /// A script that manages loading data on game start and saving it when you close the game
    /// </summary>
    [DefaultExecutionOrder(Execution_Order.save)]
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
                string currentSavePath = Path.Combine(saveFolderPath, "Current");

                try
                {
                    SaveFolder currentSaveFolder = new(currentSavePath, encryptionKey);

                    string regex = "([0-9]+\\.)+[0-9]*";
                    List<string> allDirectories = Directory.GetDirectories(saveFolderPath).Where(x => Regex.IsMatch(x, regex)).ToList();

                    // Enters statement if save folder is not compatible anymore
                    if (currentSaveFolder.ValidateSaves())
                    {
                        Debug.LogWarning("Current save folder failed validation");

                        int versionCompare = String_Utilities.CompareVersions(currentSaveFolder.GetVersion(), Application.version);

                        // Current is newer version than the save files version
                        if (versionCompare > 0)
                        {
                            Debug.LogWarning("Current version is newer than the save file version. Creating backup of old save.");

                            //Backup data to new folder named the old version
                            string oldVersionPath = Path.Combine(saveFolderPath, currentSaveFolder.GetVersion());
                            Directory.CreateDirectory(oldVersionPath);
                            SaveFolder.CopyAll(currentSaveFolder, oldVersionPath);
                            currentSaveFolder.DeleteOutdated();
                            currentFolder = currentSaveFolder;
                        }
                        // Current is older meaning an older version of the game was launched after a newer one was already launched
                        else if (versionCompare < 0)
                        {
                            Debug.LogWarning("Current version is older than the save file version. Trying to fall back on old version.");

                            //Sort list by version number
                            allDirectories.Sort((a, b) =>
                            {
                                return String_Utilities.CompareVersions(Path.GetDirectoryName(a), Path.GetDirectoryName(b));
                            });

                            // Check if old version is compatible
                            foreach (string directory in allDirectories)
                            {
                                SaveFolder oldFolder = new(directory, encryptionKey);

                                if (!oldFolder.ValidateSaves())
                                {
                                    Debug.LogWarning("Old save folder with version " + oldFolder.GetVersion() + " succeded validation");

                                    currentFolder = oldFolder;
                                    break;
                                }
                            }

                            if (currentFolder == null)
                            {
                                Debug.LogWarning("No old save found with supported files. Creating new");

                                string oldVersionPath = Path.Combine(saveFolderPath, Application.version);
                                SaveFolder newFolder = new(oldVersionPath, encryptionKey, true);
                                currentFolder = newFolder;
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Version is identitcal but still corrupted. Creating new");
                            currentSaveFolder = new(currentSavePath, encryptionKey, true);
                            currentFolder = currentSaveFolder;
                        }
                    }
                    else
                    {
                        Debug.Log("Save file successfully loaded");

                        currentFolder = currentSaveFolder;
                    }
                }
                catch
                {
                    currentFolder = new(currentSavePath, encryptionKey, true);
                }
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