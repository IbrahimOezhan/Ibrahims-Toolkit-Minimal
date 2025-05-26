using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace TemplateTools
{
    /// <summary>
    /// Provides the functionality for the UI that displays and changes elements
    /// </summary>
    public class UI_Setting : MonoBehaviour
    {
        private bool subscribed;

        [FoldoutGroup("Interface")]
        [SerializeField] public SettingsInterfaceType interfaceType;

        [FoldoutGroup("Interface"), ShowIf("interfaceType", SettingsInterfaceType.KEY), Dropdown("Settings"), SerializeField]
        public string settingKey;

        [FoldoutGroup("Interface"), ShowIf("interfaceType", SettingsInterfaceType.REFERENCE)]
        public Setting setting;

        [FoldoutGroup("Interface"), SerializeField]
        protected SettingsType settingType;

        [FoldoutGroup("UI"), SerializeField]
        protected UI_Localization title;

        [FoldoutGroup("UI"), SerializeField]
        protected UI_Interactive value;

        [FoldoutGroup("UI"), SerializeField]
        protected UI_Localization description;

        protected virtual void OnEnable()
        {
            if (Initialize()) UpdateUI();
        }

        private void OnDestroy()
        {
            if (Localization_Manager.Instance) Localization_Manager.Instance.OnLanguageChanged -= UpdateUI;
        }

        public void Setup(string settingKey)
        {
            this.settingKey = settingKey;
        }

        public void Setup(Setting setting)
        {
            this.setting = setting;
        }

        public virtual bool Initialize()
        {
            if (interfaceType == SettingsInterfaceType.KEY)
            {
                if (settingKey == string.Empty) return false;
                if (!Settings_Manager.Instance.GetSetting(settingKey, out setting))
                {
                    return false;
                }
            }
            if (setting)
            {
                if (!subscribed)
                {
                    Localization_Manager.Instance.OnLanguageChanged += UpdateUI;
                    subscribed = true;
                }

                if (!setting.GetHasDescription())
                {
                    if (description)
                    {
                        description.gameObject.SetActive(false);
                    }
                }
                else
                {
                    description.gameObject.SetActive(true);
                    description.SetKey(setting.GetDescriptionValue());
                }

                title.SetKey(setting.GetTitleValue());

                return true;
            }
            else return false;
        }

        public virtual void ChangeValue(float _value)
        {
            setting.ChangeValue(_value);
            setting.ApplyChanges();
            UpdateUI();
        }

        public virtual void UpdateUI()
        {
            if (Initialize())
            {
                value.GetComponent<Text>().text = setting.GetDisplayValue();
                value.UpdateUI();
            }
        }

        public SettingsType GetSettingsType()
        {
            return settingType;
        }
    }
}