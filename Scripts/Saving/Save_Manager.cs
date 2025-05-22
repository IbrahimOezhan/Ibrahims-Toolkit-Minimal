using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TemplateTools
{
    /// <summary>
    /// A script that manages loading data on game start and saving it when you close the game
    /// </summary>
    [DefaultExecutionOrder(-5)]
    public class Save_Manager : Manager_Base
    {
        private const string save = "Saves";
        private const string generic = "Generic.txt";
        private const string code = "a3c9e7r3gf3d5e7";

        private string savePath;
        private string genericPath;

        private Dictionary<string, string> genericData = new();
        private List<string> loadedObjets = new();

        public static Save_Manager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else
            {
                Instance = this;

                savePath = Path.Combine(Path_Utilities.GetGamePath(), save);
                if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

                genericPath = Path.Combine(savePath, generic);
                if (!File.Exists(genericPath)) File.Create(genericPath).Close();

                using (StreamReader reader = new(genericPath))
                {
                    string json = reader.ReadToEnd();
                    if (!String_Utilities.IsEmpty(json)) genericData = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                }
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                using (StreamWriter writer = new(genericPath))
                {
                    string json = JsonSerializer.Serialize(genericData);
                    writer.Write(json);
                }
            }
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
                if (int.TryParse(genericData[key], out value))
                {
                    return true;
                }
                else
                {
                    Debug.LogWarning("Value is not an integer: " + genericData[key]);
                    return false;
                }
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
                if (float.TryParse(genericData[key], out value))
                {
                    return true;
                }
                else
                {
                    Debug.LogWarning("Value is not a float: " + genericData[key]);
                    return false;
                }
            }
            else
            {
                Debug.LogWarning("Key not found in generic data: " + key);
                return false;
            }
        }

        public T LoadObject<T>(string _name, T _defaultType, bool _decrypt = true)
        {
            if (Directory.Exists(savePath))
            {
                string _path = GetPath(_name);

                string fileContent = string.Empty;

                bool fileExists = File.Exists(_path);

                if(fileExists)
                {
                    fileContent = File.ReadAllText(_path);
                }

                if (!fileExists || String_Utilities.IsEmpty(fileContent))
                {
                    SaveObject(_name, _defaultType, _decrypt);

                    Log.SendLog("Save_Manager", "Template", "Created path: " + _path + "and returned default value");

                    return _defaultType;
                }
                else
                {
                    string _data = _decrypt ? DecryptEncrypt(fileContent) : fileContent;

                    var options = new JsonSerializerOptions
                    {
                        IncludeFields = true,
                        WriteIndented = true,
                    };

                    T _loadedData = JsonSerializer.Deserialize<T>(_data, options);
                    if (_loadedData != null) return _loadedData;
                }
            }

            Log.SendLog("Save_Manager", "Template", "Returned Default Value");
            return _defaultType;
        }

        public void SaveObject<T>(string _name, T dataToSave, bool encrypt = true)
        {
            var options = new JsonSerializerOptions
            {
                IncludeFields = true,
                WriteIndented = true,
            };

            string _rawJson = JsonSerializer.Serialize(dataToSave, options);
            string _json = encrypt ? DecryptEncrypt(_rawJson) : _rawJson;

            using (StreamWriter writer = new(GetPath(_name)))
            {
                writer.Write(_json);
            }
        }

        [Button]
        public void DeleteSaveData()
        {
            string[] files = Directory.GetFiles(savePath);

            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i]);
            }
        }

        private string GetPath(string _name)
        {
            return Path.Combine(savePath, _name + ".txt");
        }

        private string DecryptEncrypt(string _data)
        {
            string _result = "";
            for (int i = 0; i < _data.Length; i++) _result += (char)(_data[i] ^ code[i % code.Length]);
            return _result;
        }
    }
}