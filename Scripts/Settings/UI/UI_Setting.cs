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
        private Text valueText;

        [BoxGroup("Interface"), SerializeField]
        protected SettingsType settingType;

        [BoxGroup("Interface"), SerializeField]
        private SettingsInterfaceType interfaceType;

        [BoxGroup("Interface"), ShowIf("interfaceType", SettingsInterfaceType.KEY), Dropdown("Settings"), SerializeField]
        private string settingKey;

        [BoxGroup("Interface"), ShowIf("interfaceType", SettingsInterfaceType.REFERENCE),SerializeField]
        protected Setting setting;

        [BoxGroup("UI"), SerializeField]
        protected UI_Localization title;
        [BoxGroup("UI"), SerializeField]
        protected UI_Localization description;

        [BoxGroup("UI"), SerializeField]
        protected UI_Interactive value;

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
                if (String_Utilities.IsEmpty(settingKey)) return false;

                if (!Settings_Manager.Instance.GetSetting(settingKey, out setting)) return false;
            }

            if (setting == null)
            {
                return false;
            }

            if (!subscribed)
            {
                Localization_Manager.Instance.OnLanguageChanged += UpdateUI;
                subscribed = true;
            }

            Setting_Local_Json settingLocal = setting.GetLocal();

            title.SetKey(settingLocal.title);
            description.SetKey(settingLocal.description);

            return true;
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