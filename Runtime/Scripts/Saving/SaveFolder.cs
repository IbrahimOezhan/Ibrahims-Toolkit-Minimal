using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using UnityEngine;
using Application = UnityEngine.Application;

namespace IbrahKit
{
    public class SaveFolder
    {
        private const string versionPath = "Version.meta";
        private const string generic = "Generic.txt";
        private const string saveExtension = ".save";
        private const string saveFileRegex = ".*\\.save";

        private string encryptionKey;
        private string folderPath;
        private string[] outdatedFiles;

        private Dictionary<string, string> genericData = new();

        JsonSerializerOptions genericOptions = new()
        {
            IncludeFields = true,
            WriteIndented = true,
        };

        public void SaveGenericData()
        {
            string json = JsonSerializer.Serialize(genericData, genericOptions);
            File_Utilities.WriteToFile(Path.Combine(folderPath, generic), json);
        }

        public SaveFolder(string folderPath, string encryptionKey, bool deleteAll = false)
        {
            this.folderPath = folderPath;

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Debug.Log("Created missing directory: " + folderPath);
            }

            if (deleteAll)
            {
                string[] files = GetFiles();

                for (int i = 0; i < files.Length; i++)
                {
                    File.Delete(files[i]);
                }
            }

            string vPath = Path.Combine(folderPath, versionPath);
            string gPath = Path.Combine(folderPath, generic);

            File_Utilities.WriteToFile(vPath, Application.version, true);

            File_Utilities.WriteToFile(gPath, string.Empty, true);

            this.encryptionKey = encryptionKey;
        }

        public string GetVersion()
        {
            return File_Utilities.ReadFromFile(Path.Combine(folderPath, versionPath));
        }

        public string[] GetFiles()
        {
            return Directory.GetFiles(folderPath);
        }

        public string[] GetFiles(string regex)
        {
            return Directory.GetFiles(folderPath).Where(x => Regex.IsMatch(x, regex)).ToArray();
        }

        public bool ValidateSaves()
        {
            string[] files = GetFiles(saveFileRegex);
            string[] fileContents = new string[files.Length];
            outdatedFiles = new string[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                fileContents[i] = File_Utilities.ReadFromFile(files[i]);

                if (String_Utilities.IsEmpty(fileContents[i]))
                {
                    Debug.Log("File contents are empty for " + files[i]);
                    continue;
                }

                bool tryParse = Parse_Utilties.IsValidJson(fileContents[i]);

                if (!tryParse)
                {
                    fileContents[i] = String_Utilities.DecryptEncrypt(fileContents[i], encryptionKey);

                    Debug.Log("File " + files[i] + " probably encrypted. Attemping decryption");

                    tryParse = Parse_Utilties.IsValidJson(fileContents[i]);

                    if (!tryParse)
                    {
                        Debug.LogError("File still not in json format after decryption. Probably damaged");
                    }
                }

                Savable s = GetSavable(fileContents[i]);

                string fullName = s.fullName;

                try
                {
                    Savable derived = GetDerivedSavable(fileContents[i], s);

                    Debug.Log("Deserialization successfull");
                }
                catch (Exception ex)
                {
                    Debug.Log("Deserialization failed with type: " + fullName + " " + ex.Message);

                    outdatedFiles[i] = files[i];

                    return true;
                }
            }

            return false;
        }

        public void DeleteOutdated()
        {
            string[] outdated = outdatedFiles;

            for (int i = 0; i < outdated.Length; i++)
            {
                if (String_Utilities.IsEmpty(outdated[i])) continue;

                File.Delete(outdated[i]);
            }
        }

        public static void CopyAll(SaveFolder from, string folderPath)
        {
            string[] files = Directory.GetFiles(from.folderPath);

            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i], Path.Combine(folderPath, Path.GetFileName(files[i])), true);
            }
        }

        public Savable LoadObject(string _name, Savable _defaultType, bool _decrypt = true)
        {
            if (!Directory.Exists(folderPath))
            {
                Debug.Log("Returned Default Value for save: " + _name);

                return _defaultType;
            }

            string _path = Path.Combine(folderPath, _name + saveExtension);

            string fileContent = File_Utilities.ReadFromFile(_path);

            if (String_Utilities.IsEmpty(fileContent))
            {
                SaveObject(_name, _defaultType, _decrypt);

                Debug.Log("Created path: " + _path + "and returned default value");

                return _defaultType;
            }

            fileContent = _decrypt ? String_Utilities.DecryptEncrypt(fileContent, encryptionKey) : fileContent;

            Debug.Log("Read file content " + fileContent + " for " + _name);

            Savable savable = GetSavable(fileContent);

            Savable derived = GetDerivedSavable(fileContent, savable);

            Debug.Log(JsonUtility.ToJson(savable) + " " + JsonUtility.ToJson(derived));

            if (derived == null)
            {
                Debug.LogWarning("Was not able to deserialize. Fallback to default");
                return _defaultType;
            }

            Debug.Log("Successfully loaded data");

            return derived;
        }

        public void SaveObject(string _name, Savable dataToSave, bool encrypt = true)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IncludeFields = true,
                WriteIndented = true,
            };

            Type type = Type.GetType(dataToSave.fullName);

            string _rawJson = JsonSerializer.Serialize(dataToSave, type, options);

            Debug.Log("Saved file content " + _rawJson + " for " + _name);

            string _json = encrypt ? String_Utilities.DecryptEncrypt(_rawJson, encryptionKey) : _rawJson;

            string _path = Path.Combine(folderPath, _name + saveExtension);

            File_Utilities.WriteToFile(_path, _json);
        }

        private Savable GetSavable(string json)
        {
            JsonSerializerOptions genericOptions = new()
            {
                IncludeFields = true,
                WriteIndented = true,
            };

            return JsonSerializer.Deserialize<Savable>(json, genericOptions);
        }

        private Savable GetDerivedSavable(string json, Savable type)
        {
            if (String_Utilities.IsEmpty(json))
            {
                Debug.LogWarning("Passed json is null or empty");
                return null;
            }

            if (type == null)
            {
                Debug.LogWarning("Passed type is null");
                return null;
            }

            JsonSerializerOptions genericOptions = new()
            {
                IncludeFields = true,
                WriteIndented = true,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            };

            Type instanceType = Type.GetType(type.fullName);

            Debug.Log(instanceType);

            object o = JsonSerializer.Deserialize(json, instanceType, genericOptions);

            return (Savable)o;
        }

        public int WriteValue(string key, string value)
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

            Debug.LogWarning("Key not found in generic data: " + key);
            return false;
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

            Debug.LogWarning("Key not found in generic data: " + key);
            return false;
        }
    }
}