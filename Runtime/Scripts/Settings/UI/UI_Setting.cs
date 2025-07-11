using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace IbrahKit
{
    /// <summary>
    /// Provides the functionality for the UI that displays and changes elements
    /// </summary>
    public class UI_Setting : MonoBehaviour
    {
        protected Setting setting;

        private bool initialized = false;
        
        private bool subscribed;

        [BoxGroup("Setting"), SerializeField]
        protected SettingsType settingType;

        [BoxGroup("Setting"), SerializeField]
        private SettingsInterfaceType interfaceType;

        [BoxGroup("Setting"), ShowIf("interfaceType", SettingsInterfaceType.KEY), Dropdown("Settings"), SerializeField]
        private string settingKey;

        [BoxGroup("Setting"), ShowIf("interfaceType", SettingsInterfaceType.LOCALREFERENCE), SerializeField]
        protected Setting_Container localReference;

        [BoxGroup("Setting"), ShowIf("interfaceType", SettingsInterfaceType.LOCAL), SerializeField, OnValueChanged("OnValueChanged"), ValueDropdown("GetAllTypesDropdownFormat")]
        private string extension = "None";

        [BoxGroup("Setting"), ShowIf("interfaceType", SettingsInterfaceType.LOCAL), SerializeField, SerializeReference]
        protected Setting localSetting;

        [BoxGroup("UI"), SerializeField]
        protected UI_Text_Setter_Legacy title;

        [BoxGroup("UI"), SerializeField]
        protected UI_Text_Setter_Legacy description;

        [BoxGroup("UI"), SerializeField]
        protected UI_Text_Setter_Legacy value;

        protected virtual void OnEnable()
        {
            if (Initialize()) UpdateUI();
        }

        private void OnDestroy()
        {
            if (Localization_Manager.Instance) Localization_Manager.Instance.OnLanguageChanged -= UpdateUI;
        }

        //Invoked by ODIN
        private void OnValueChanged()
        {
            List<Type> types = Type_Utilities.GetAllTypes(typeof(Setting)).ToList();

            Type type = types.Find(x => x.Name == extension);

            if (type != null)
            {
                localSetting = (Setting)Activator.CreateInstance(type);
            }

            extension = "None";
        }

        //Invoked by ODIN
        private IEnumerable GetAllTypesDropdownFormat() { return Type_Utilities.GetAllTypesDropdownFormat(typeof(Setting)); }

        public void Setup(string settingKey)
        {
            this.settingKey = settingKey;
        }

        public void Setup(Setting_Container setting)
        {
            this.setting = setting.GetSetting();
        }

        public virtual void ChangeValue(float _value)
        {
            setting.SetValue(setting.GetValue() + _value);
            setting.ApplyChanges();
            UpdateUI();
        }

        public virtual void UpdateUI()
        {
            if (Initialize())
            {
                value.SetText(setting.GetDisplayValue());
            }
        }

        public Setting GetSetting()
        {
            return setting;
        }

        public SettingsType GetSettingsType()
        {
            return settingType;
        }

        public virtual bool Initialize()
        {
            if (initialized) return true;
            
            switch (interfaceType)
            {
                case SettingsInterfaceType.LOCALREFERENCE:
                    setting = localReference.GetSetting();
                    break;
                case SettingsInterfaceType.KEY:
                    Settings_Manager.Instance.GetSetting(settingKey, out setting);
                    break;
                case SettingsInterfaceType.LOCAL:
                    setting = localSetting;
                    break;
            }

            if (setting == null)
            {
                return false;
            }

            setting.Init("");

            if (!subscribed)
            {
                Localization_Manager.Instance.OnLanguageChanged += UpdateUI;
                subscribed = true;
            }

            Setting_Local_Json settingLocal = setting.GetLocal();

            if (settingLocal == null)
            {
                Debug.LogWarning("Local json is null");
                return false;
            }

            bool titleEmpty = String_Utilities.IsEmpty(settingLocal.title);
            bool titleNull = title == null;

            if (titleEmpty)
            {
                Debug.LogWarning("Title is empty");
            }

            if (titleNull)
            {
                Debug.LogWarning("Title is null");
            }

            if (!titleEmpty && !titleNull)
            {
                title.SetText(settingLocal.title);
            }

            if (description != null && !String_Utilities.IsEmpty(settingLocal.description))
            {
                description.SetText(settingLocal.description);
            }

            initialized = true;
            
            return true;
        }
    }
}