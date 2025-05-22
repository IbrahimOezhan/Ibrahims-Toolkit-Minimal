using System;
using System.Collections.Generic;
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

                data = Save_Manager.Instance.LoadObject<SaveData>("Settings", new(settings.Count));

                if (data.data.Count != settings.Count)
                {
                    data.data.Clear();
                    while (data.data.Count != settings.Count) data.data.Add("");
                }

                for (int i = 0; i < settings.Count; i++)
                {
                    settings[i].LoadSetting(data.data[i]);
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
                    data.data[i] = settings[i].value.ToString();
                }
                Save_Manager.Instance.SaveObject("Settings", data);
            }
        }

        private void OnValidate()
        {
            List<string> _keys = new();
            for (int i = 0; i < settings.Count; i++) _keys.Add(settings[i].settingsKey);
            String_Utilities.CreateDropdown(_keys, "Settings");
        }

        public void OpenSettings(UI_Menu_Basic _origin)
        {
            _origin.MenuTransition(Menu_Settings.Instance, _origin);
        }

        public bool GetSettingByKey(string _key, out Setting setting)
        {
            setting = settings.Find(x => x.settingsKey.Equals(_key));

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
            public List<string> data = new();

            public SaveData()
            {

            }

            public SaveData(int count)
            {
                for (int i = 0; i < count; i++) data.Add("");
            }
        }
    }
}