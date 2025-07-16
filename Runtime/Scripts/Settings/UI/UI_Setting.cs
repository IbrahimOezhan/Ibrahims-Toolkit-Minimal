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
        [TabGroup("Setting")]
        [SerializeField, LabelText("Setting Type")]
        protected SettingsType settingType;

        [TabGroup("Setting")]
        [SerializeField, LabelText("Interface Type")]
        private SettingsInterfaceType interfaceType;

        [TabGroup("Setting")]
        [ShowIf(nameof(interfaceType), SettingsInterfaceType.KEY)]
        [Dropdown("Settings_Manager.KEY")]
        [SerializeField, LabelText("Setting Key")]
        private string settingKey;

        [TabGroup("Setting")]
        [ShowIf(nameof(interfaceType), SettingsInterfaceType.LOCALREFERENCE)]
        [SerializeField, LabelText("Local Reference")]
        protected Setting_Container localReference;

        [TabGroup("Setting")]
        [ShowIf(nameof(interfaceType), SettingsInterfaceType.LOCAL)]
        [SerializeField, OnValueChanged(nameof(OnValueChanged))]
        [ValueDropdown(nameof(GetAllTypesDropdownFormat))]
        [LabelText("Extension")]
        private string extension = "None";

        [TabGroup("Setting")]
        [ShowIf(nameof(interfaceType), SettingsInterfaceType.LOCAL)]
        [SerializeField, SerializeReference, LabelText("Local Setting")]
        protected Setting localSetting;

        [TabGroup("UI")]
        [SerializeField, LabelText("Title")]
        protected UI_Text_Setter_Legacy title;

        [TabGroup("UI")]
        [SerializeField, LabelText("Description")]
        protected UI_Text_Setter_Legacy description;

        [TabGroup("UI")]
        [SerializeField, LabelText("Value")]
        protected UI_Text_Setter_Legacy value;

        [TabGroup("Runtime")]
        [SerializeField, ReadOnly, LabelText("Initialized")]
        protected bool initialized = false;

        [TabGroup("Runtime")]
        [SerializeField, ReadOnly, LabelText("Subscribed")]
        private bool subscribed;

        [TabGroup("Runtime")]
        [SerializeField, ReadOnly, LabelText("Setting")]
        protected Setting setting;

        protected virtual void OnEnable()
        {
            if (!initialized)
            {
                Debug.LogWarning("Initialization failed");
                return;
            }

            UpdateUI();
        }

        private void OnDestroy()
        {
            if (Localization_Manager.Exists(out Localization_Manager lm, true))
            {
                lm.OnLanguageChanged -= UpdateUI;
            }
        }

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

        public void Setup(string settingKey)
        {
            this.settingKey = settingKey;

            Setup();
        }

        public void Setup(Setting_Container setting)
        {
            this.setting = setting.GetSetting();

            Setup();
        }

        public virtual void Setup()
        {
            Initialize();

            if (!initialized)
            {
                Debug.LogWarning("Initialization failed");
                return;
            }

            UpdateUI();
        }

        public virtual void ChangeValue(float _value)
        {
            setting.SetValue(setting.GetValue() + _value);

            setting.ApplyChanges();

            UpdateUI();
        }

        public virtual void UpdateUI()
        {
            if (!initialized) Initialize();
            if (!initialized)
            {
                Debug.LogWarning("Initialization failed");
                return;
            }

            value.SetText(setting.GetDisplayValue());
        }

        public Setting GetSetting()
        {
            return setting;
        }

        public SettingsType GetSettingsType()
        {
            return settingType;
        }

        private IEnumerable GetAllTypesDropdownFormat()
        {
            return Type_Utilities.GetAllTypesDropdownFormat(typeof(Setting));
        }

        public virtual bool Initialize()
        {
            if (initialized)
            {
                Debug.LogWarning("Already initialized");
                return true;
            }

            if (value == null)
            {
                Debug.LogWarning($"{nameof(value)} is null");
                return false;
            }

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
                Debug.LogWarning($"{nameof(setting)} is null");
                return false;
            }

            setting.Init("");

            if (!subscribed)
            {
                Localization_Manager.Instance.OnLanguageChanged += UpdateUI;
                subscribed = true;
            }
            else
            {
                Debug.LogWarning($"Already subscribed to {nameof(Localization_Manager)}");
            }

            Setting_Local_Json settingLocal = setting.GetLocal();

            if (settingLocal == null)
            {
                Debug.LogWarning($"{nameof(settingLocal)} is null");
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

            Debug.LogWarning("Ínitialized successfully");

            return true;
        }
    }
}