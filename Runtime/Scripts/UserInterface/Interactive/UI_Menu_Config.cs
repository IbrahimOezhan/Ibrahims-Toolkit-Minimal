using System.Collections.Generic;
using UnityEngine;

namespace IbrahKit
{
    [CreateAssetMenu(fileName = "NewUIMenuConfig", menuName = "IbrahKit/UI MenuConfig")]
    public class UI_Menu_Config : ScriptableObject
    {
        [SerializeField] private UI_Menu_Button menuButtonPrefab;

        [SerializeField] private List<UI_Setting> settingPrefabs;

        public UI_Menu_Button GetMenuButton()
        {
            return menuButtonPrefab;
        }

        public UI_Setting GetSettingsPrefab(SettingsType settingsType)
        {
            return settingPrefabs.Find(x => x.GetSettingsType() == settingsType);
        }
    }
}