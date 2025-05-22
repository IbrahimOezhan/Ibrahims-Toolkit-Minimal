using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace TemplateTools
{
    [DefaultExecutionOrder(-2)]
    public class Settings_Manager : Manager_Base
    {
        private SaveData data;

        [SerializeField] private List<Setting> settings;

        public UniversalRenderPipelineAsset pipelineAsset;

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
                settings.RemoveAll(x => !x.gameObject.activeInHierarchy);

                data = Save_Manager.Instance.LoadObject<SaveData>("Settings", new());

                for(int i  = 0; i < settings.Count; i++)
                {
                    string key = settings[i].GetKey();

                    if (!data.data.ContainsKey(key))
                    {
                        data.data.Add(key, "");
                    }

                    settings[i].LoadSetting(data.data[key]);
                    settings[i].ApplyChanges();
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

                    if(data.data.ContainsKey(key))
                    {
                        data.data[key] = settings[i].GetValue().ToString();
                    }
                }

                Save_Manager.Instance.SaveObject("Settings", data);
            }
        }

        private void OnValidate()
        {
            String_Utilities.CreateDropdown(settings.Select(x => x.GetKey()).ToList(), "Settings");
        }

        public void OpenSettings(UI_Menu_Basic _origin)
        {
            if(_origin == null)
            {
                Debug.LogWarning("Provided origin menu is null");
                return;
            }

            _origin.MenuTransition(Menu_Settings.Instance, _origin);
        }

        public bool GetSetting(string _key, out Setting setting)
        {
            setting = null;

            if(String_Utilities.IsEmpty(_key))
            {
                Debug.LogWarning("Provided key is empty or null");
                return false;
            }

            setting = settings.Find(x => x.GetKey().Equals(_key));

            if(setting == null)
            {
                Debug.LogWarning("No setting found with key: " + _key);
                return false;
            }

            return true;
        }

        [Serializable]
        private class SaveData
        {
            public Dictionary<string,string> data = new();
        }
    }
}