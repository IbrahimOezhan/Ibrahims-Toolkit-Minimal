using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace TemplateTools
{
    [System.Serializable]
    public class Menu_Item_Setting : Menu_Item_Base
    {
        [SerializeField] private SettingsInterfaceType settingType;

        [ShowIf("settingType", SettingsInterfaceType.REFERENCE),SerializeField]
        private Setting reference;

        [ShowIf("settingType", SettingsInterfaceType.KEY), Dropdown("Settings"),SerializeField]
        private string settingsKey;

        public override void Spawn(RectTransform parent, UI_Menu_Extended menu)
        {
            UI_Menu_Config config = menu.GetMenuConfig();

            if (!Settings_Manager.Instance.GetSetting(settingsKey, out Setting _foundSetting))
            {
                return;
            }

            UI_Setting setting = config.GetSettingsPrefab(_foundSetting.GetSettingsType());

            UI_Setting settingInstance = GameObject.Instantiate(setting, parent);

            spawnedObject = settingInstance.gameObject;

            switch (settingType)
            {
                case SettingsInterfaceType.KEY:
                    settingInstance.Setup(settingsKey);
                    break;
                case SettingsInterfaceType.REFERENCE:
                    settingInstance.Setup(reference);
                    break;
            }

            settingInstance.UpdateUI();
        }
    }
}