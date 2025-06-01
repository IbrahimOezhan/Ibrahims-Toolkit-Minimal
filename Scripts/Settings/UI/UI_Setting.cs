using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace TemplateTools
{
    /// <summary>
    /// Provides the functionality for the UI that displays and changes elements
    /// </summary>
    public class UI_Setting : MonoBehaviour
    {
        protected Setting setting;

        private bool subscribed;

        [BoxGroup("Interface"), SerializeField]
        protected SettingsType settingType;

        [BoxGroup("Interface"), SerializeField]
        private SettingsInterfaceType interfaceType;

        [BoxGroup("Interface"), ShowIf("interfaceType", SettingsInterfaceType.KEY), Dropdown("Settings"), SerializeField]
        private string settingKey;

        [BoxGroup("Interface"), ShowIf("interfaceType", SettingsInterfaceType.LOCAL),SerializeField]
        protected Setting_Container localSetting;

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
            switch(interfaceType)
            {
                case SettingsInterfaceType.LOCAL:
                    setting = localSetting.GetSetting();
                    break;
                case SettingsInterfaceType.KEY:
                    Settings_Manager.Instance.GetSetting(settingKey, out setting);
                    break;
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

            if(settingLocal == null)
            {
                Debug.LogWarning("Local json is null");
                return false;
            }

            if(String_Utilities.IsEmpty(settingLocal.title))
            {
                Debug.LogWarning("Title is empty");
                return false;
            }

            if(title == null)
            {
                Debug.LogWarning("Title is null");
                return false;
            }

            title.SetText(settingLocal.title);

            if (description != null && !String_Utilities.IsEmpty(settingLocal.description))
            {
                description.SetText(settingLocal.description);
            }

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