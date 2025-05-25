using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
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
                Debug.Log("Created missing file: " + vPath);
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
                    Debug.Log("Validate: Contains non save file. Continueing");
                    continue;
                }

                if (String_Utilities.IsEmpty(fileContents[i]))
                {
                    Debug.Log("Validate: Is Empty. Continueing");
                    continue;
                }

                bool tryParse = Parse_Utilties.IsValidJson(fileContents[i]);

                string decrypted = tryParse ? fileContents[i] : String_Utilities.DecryptEncrypt(fileContents[i], encryptionKey);

                Debug.Log("FileContents in " + files[i] + " : " + decrypted);

                Savable s = JsonSerializer.Deserialize<Savable>(decrypted, genericOptions);

                string fullName = s.fullName;

                try
                {
                    Type? type = Type.GetType(fullName);

                    if (type == null) Debug.LogWarning("Couldnt get type of type: " + fullName);

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        IncludeFields = true,
                        WriteIndented = true,
                        UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Disallow,
                    };

                    Type converterType = typeof(JsonAliasConverter<>).MakeGenericType(type);

                    // Create an instance (assuming parameterless constructor)
                    JsonConverter converterInstance = (JsonConverter)Activator.CreateInstance(converterType);

                    options.Converters.Add(converterInstance);

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

        public T LoadObject<T>(string _name, T _defaultType, bool _decrypt = true)
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

            string _data = _decrypt ? String_Utilities.DecryptEncrypt(fileContent, encryptionKey) : fileContent;

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
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IncludeFields = true,
                WriteIndented = true,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            };

            string _rawJson = JsonSerializer.Serialize(dataToSave, options);

            string _json = encrypt ? String_Utilities.DecryptEncrypt(_rawJson, encryptionKey) : _rawJson;

            string _path = Path.Combine(folderPath, _name + ".txt");

            File_Utilities.WriteToFile(_path, _json);
        }

        private bool TryDeserialize(string json, Type type, JsonSerializerOptions options, out object result)
        {
            result = null;

            try
            {
                result = JsonSerializer.Deserialize(json, type, options);
                return true;
            }
            catch (Exception e)
            {
                result = null;
                return false;
            }
        }
    }
}