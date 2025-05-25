using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application = UnityEngine.Application;

namespace TemplateTools
{
    public class SaveFolder
    {
        private const string versionPath = "Version.txt";
        private const string generic = "Generic.txt";

        private string encryptionKey;
        private string folderPath;

        private Dictionary<string, string> genericData = new();
        private List<string> nonSaveFiles = new();

        JsonSerializerOptions genericOptions = new JsonSerializerOptions
        {
            IncludeFields = true,
            WriteIndented = true,
        };

        public void SaveGenericData()
        {
            string json = JsonSerializer.Serialize(genericData, genericOptions);
            File_Utilities.WriteToFile(Path.Combine(folderPath, generic), json);
        }

        public SaveFolder(string folderPath, string encryptionKey)
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
                File_Utilities.WriteToFile(vPath, Application.version);
            }

            if (!File.Exists(gPath))
            {
                using StreamWriter writer = new(gPath);
                Debug.Log("Created missing file: " + gPath);
            }

            this.encryptionKey = encryptionKey;
        }

        public string GetVersion()
        {
            return File_Utilities.ReadFromFile(Path.Combine(folderPath, versionPath));
        }

        public bool ValidateSaves()
        {
            string[] files = Directory.GetFiles(folderPath);
            string[] fileContents = new string[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                fileContents[i] = File_Utilities.ReadFromFile(files[i]);

                if (nonSaveFiles.Contains(files[i]))
                {
                    Debug.Log("Skipping "+ files[i]);
                    continue;
                }

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

                    return true;
                }
            }

            return false;
        }

        public Savable LoadObject(string _name, Savable _defaultType, bool _decrypt = true)
        {
            if (!Directory.Exists(folderPath))
            {
                Debug.Log("Returned Default Value for save: " + _name);

                return _defaultType;
            }

            string _path = Path.Combine(folderPath, _name + ".txt");

            string fileContent = File_Utilities.ReadFromFile(_path);

            if (String_Utilities.IsEmpty(fileContent))
            {
                SaveObject(_name, _defaultType, _decrypt);

                Debug.Log("Created path: " + _path + "and returned default value");

                return _defaultType;
            }

            fileContent = _decrypt ? String_Utilities.DecryptEncrypt(fileContent, encryptionKey) : fileContent;

            Savable savable = GetSavable(fileContent);

            Savable derived = GetDerivedSavable(fileContent, savable);

            if (derived != null)
            {
                Debug.Log("Successfully loaded data");
                return derived;
            }

            Debug.LogWarning("Was not able to deserialize. Fallback to default");

            return _defaultType;
        }

        public void SaveObject(string _name, Savable dataToSave, bool encrypt = true)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IncludeFields = true,
                WriteIndented = true,
            };

            string _rawJson = JsonSerializer.Serialize(dataToSave, options);

            string _json = encrypt ? String_Utilities.DecryptEncrypt(_rawJson, encryptionKey) : _rawJson;

            string _path = Path.Combine(folderPath, _name + ".txt");

            File_Utilities.WriteToFile(_path, _json);
        }

        private void AddAliasConvert(JsonSerializerOptions options, Type type)
        {
            Type converterType = typeof(JsonAliasConverter<>).MakeGenericType(type);

            JsonConverter converterInstance = (JsonConverter)Activator.CreateInstance(converterType);

            options.Converters.Add(converterInstance);
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
            JsonSerializerOptions genericOptions = new()
            {
                IncludeFields = true,
                WriteIndented = true,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            };

            Type instanceType = Type.GetType(type.fullName);

            AddAliasConvert(genericOptions, instanceType);

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