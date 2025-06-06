using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using UnityEngine;

namespace IbrahKit
{
    [DefaultExecutionOrder(Execution_Order.settings)]
    public class Settings_Manager : Manager_Base
    {
        private SaveData data;

        [SerializeField, OnValueChanged("OnValueChanged"), ValueDropdown("GetAllTypesDropdownFormat")]
        private string addSetting = "None";

        [SerializeReference] private List<Setting> settings = new();

        public static Settings_Manager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            if (Instance == this)
            {
                data = (SaveData)Save_Manager.currentFolder.LoadObject("Settings", new SaveData());

                for (int i = 0; i < settings.Count; i++)
                {
                    string key = settings[i].GetKey();
                    settings[i].Init(data.GetValue(key));
                }
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                for (int i = 0; i < settings.Count; i++)
                {
                    string key = settings[i].GetKey();

                    data.SetValue(key, settings[i].GetValue().ToString());
                }

                Save_Manager.currentFolder.SaveObject("Settings", data);
            }
        }

        [Button(Name = "Validate")]
        private void OnValidate()
        {
            String_Utilities.CreateDropdown(settings.Select(x => x.GetKey()).ToList(), "Settings");
        }

        //Invoked by Odin
        private IEnumerable GetAllTypesDropdownFormat() { return Type_Utilities.GetAllTypesDropdownFormat(typeof(Setting)); }

        //Invoked by Odin
        private void OnValueChanged()
        {
            if (addSetting == "None") return;

            List<Type> types = Type_Utilities.GetAllTypes(typeof(Setting)).ToList();

            Type type = types.Find(x => x.Name == addSetting);

            if (type != null)
            {
                settings.Add((Setting)Activator.CreateInstance(type));
            }

            addSetting = "None";
        }

        public void OpenSettings(UI_Menu_Basic _origin)
        {
            if (_origin == null)
            {
                Debug.LogWarning("Provided origin menu is null");
                return;
            }

            _origin.MenuTransition(Menu_Settings.Instance, _origin);
        }

        public bool GetSetting(string _key, out Setting setting)
        {
            setting = null;

            if (String_Utilities.IsEmpty(_key))
            {
                Debug.LogWarning("Provided key is empty or null");
                return false;
            }

            setting = settings.Select(x => x).ToList().Find(x => x.GetKey().Equals(_key));

            if (setting == null)
            {
                Debug.LogWarning("No setting found with key: " + _key);
                return false;
            }

            return true;
        }

        [Serializable]
        private class SaveData : Savable
        {
            [JsonInclude]
            private Dictionary<string, string> data = new();

            public string GetValue(string key)
            {
                if (data.TryGetValue(key, out string value))
                {
                    return value;
                }

                return "";
            }

            public void SetValue(string key, string value)
            {
                if (data.ContainsKey(key))
                {
                    data[key] = value;
                }
                else data.TryAdd(key, value);
            }
        }
    }
}