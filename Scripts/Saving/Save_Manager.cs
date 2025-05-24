using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using UnityEngine;
using Application = UnityEngine.Application;

namespace TemplateTools
{
    /// <summary>
    /// A script that manages loading data on game start and saving it when you close the game
    /// </summary>
    [DefaultExecutionOrder(-5)]
    public class Save_Manager : Manager_Base
    {
        private const string code = "a3c9e7r3gf3d5e7";

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

                SaveFolder saveFolder = new(currentSaveFolder);

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
                        SaveFolder folder = new(directory);
                        if (!folder.ValidateSaves())
                        {
                            Debug.LogWarning("Folder with version " + folder.GetVersion() + " succeded validation");
                            currentFolder = folder;
                            break;
                        }
                    }

                    Directory.Move(currentSaveFolder, Path.Combine(saveFolderPath, saveFolder.GetVersion()));

                    saveFolder = new(currentSaveFolder);

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

        public class SaveFolder
        {
            public string folderPath;

            private string versionPath = "Version.txt";

            private string generic = "Generic.txt";

            private Dictionary<string, string> genericData = new();
            private List<string> nonSaveFiles = new();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IncludeFields = true,
                WriteIndented = true,
                UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow
            };

            JsonSerializerOptions options2 = new JsonSerializerOptions
            {
                IncludeFields = true,
                WriteIndented = true,
            };

            public void SaveGenericData()
            {
                string json = JsonSerializer.Serialize(genericData, options);
                WriteToFile(Path.Combine(folderPath,generic), json);
            }

            public SaveFolder(string folderPath)
            {
                this.folderPath = folderPath;

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    Debug.Log("Created missing directory: " + folderPath);
                }

                string vPath = Path.Combine(folderPath, versionPath);
                string gPath = Path.Combine(folderPath, generic);

                nonSaveFiles.Add(vPath);
                nonSaveFiles.Add(gPath);

                if (!File.Exists(vPath))
                {
                    WriteToFile(vPath, Application.version);
                    Debug.Log("Created missing file: " + vPath);
                }

                if (!File.Exists(gPath))
                {
                    using StreamWriter writer = new(gPath);
                    Debug.Log("Created missing file: " + gPath);
                }
            }

            public string GetVersion()
            {
                return ReadFromFile(Path.Combine(folderPath, versionPath));
            }

            public bool ValidateSaves()
            {
                string[] files = Directory.GetFiles(folderPath);
                string[] fileContents = new string[files.Length];

                for (int i = 0; i < files.Length; i++)
                {
                    fileContents[i] = ReadFromFile(files[i]);

                    if (nonSaveFiles.Contains(files[i]))
                    {
                        Debug.Log("Validate: Contains non save file. Continueing");
                        continue;
                    }

                    if (String_Utilities.IsEmpty(fileContents[i]))
                    {
                        Debug.Log("Validate: Is Empty. Continueing");
                        continue;
                    }

                    string decrypted = String_Utilities.DecryptEncrypt(fileContents[i], code);

                    Debug.Log("FileContents in " +  files[i] + " : " + decrypted);

                    Savable s = JsonSerializer.Deserialize<Savable>(decrypted, options2);

                    string fullName = s.fullName;

                    try
                    {
                        Type? type = Type.GetType(fullName);

                        if (type == null) Debug.LogWarning("Couldnt get type of type: " + fullName);

                        object obj = JsonSerializer.Deserialize(decrypted, type, options);

                        Debug.Log("Successfull deserialization");
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("Deserialize failed with type: " + fullName + " " + ex.Message);

                        return true;
                    }
                }

                return false;
            }

            public int WriteValue(string key, string value)
            {
                try
                {
                    if (genericData.ContainsKey(key))
                    {
                        genericData[key] = value;
                        return 1;
                    }
                    else
                    {
                        genericData.Add(key, value);
                        return 0;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Error writing value to generic data: " + e.Message);
                    return -1;
                }
            }

            public bool ReadString(string key, out string value)
            {
                value = string.Empty;

                if (genericData.ContainsKey(key))
                {
                    value = genericData[key];
                    return true;
                }
                else
                {
                    Debug.LogWarning("Key not found in generic data: " + key);
                    return false;
                }
            }

            public bool ReadInt(string key, out int value)
            {
                value = 0;

                if (genericData.ContainsKey(key))
                {
                    if (!int.TryParse(genericData[key], out value))
                    {
                        Debug.LogWarning("Value is not an integer: " + genericData[key]);
                        return false;
                    }

                    return true;
                }
                else
                {
                    Debug.LogWarning("Key not found in generic data: " + key);
                    return false;
                }
            }

            public bool ReadFloat(string key, out float value)
            {
                value = 0;

                if (genericData.ContainsKey(key))
                {
                    if (!float.TryParse(genericData[key], out value))
                    {
                        Debug.LogWarning("Value is not a float: " + genericData[key]);
                        return false;

                    }
                    return true;
                }
                else
                {
                    Debug.LogWarning("Key not found in generic data: " + key);
                    return false;
                }
            }

            public T LoadObject<T>(string _name, T _defaultType, bool _decrypt = true)
            {
                if (!Directory.Exists(folderPath))
                {
                    Debug.Log("Returned Default Value for save: " + _name);

                    return _defaultType;
                }

                string _path = Path.Combine(folderPath,_name + ".txt");

                string fileContent = ReadFromFile(_path);

                if (String_Utilities.IsEmpty(fileContent))
                {
                    SaveObject(_name, _defaultType, _decrypt);

                    Debug.Log("Created path: " + _path + "and returned default value");

                    return _defaultType;
                }

                string _data = _decrypt ? String_Utilities.DecryptEncrypt(fileContent, code) : fileContent;

                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                    WriteIndented = true,
                };

                T _loadedData = JsonSerializer.Deserialize<T>(_data, options);

                if (_loadedData != null)
                {
                    Debug.LogWarning("Successfully loaded data: " + _data);
                    return _loadedData;
                }

                Debug.LogWarning("Was not able to deserialize. Fallback to default");

                return _defaultType;
            }

            public void SaveObject<T>(string _name, T dataToSave, bool encrypt = true)
            {
                string _rawJson = JsonSerializer.Serialize(dataToSave, options);

                string _json = encrypt ? String_Utilities.DecryptEncrypt(_rawJson, code) : _rawJson;

                WriteToFile(Path.Combine(folderPath, _name + ".txt"), _json);

                Debug.Log("Successfully saved: " + _name + "\n" + _rawJson);
            }

            private void WriteToFile(string filePath, string fileContent)
            {
                using (StreamWriter writer = new(filePath))
                {
                    writer.Write(fileContent);
                }
            }

            private string ReadFromFile(string filePath)
            {
                string fileContent = string.Empty;

                bool fileExists = File.Exists(filePath);

                if (fileExists)
                {
                    using StreamReader reader = new(filePath);
                    fileContent = reader.ReadToEnd();
                }

                return fileContent;
            }
        }
    }
}