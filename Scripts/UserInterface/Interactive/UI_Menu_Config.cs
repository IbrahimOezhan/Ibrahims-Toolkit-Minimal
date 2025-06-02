using System.Collections.Generic;
using UnityEngine;

namespace IbrahKit
{
    [CreateAssetMenu(fileName = "NewUIMenuConfig", menuName = "ScriptableObjects/UI_MenuConfig")]
    public class UI_Menu_Config : ScriptableObject
    {
        public UI_Menu_Button menuButtonPrefab;

        public List<UI_Setting> settingPrefabs;

        public UI_Setting GetSettingsPrefab(SettingsType settingsType)
        {
            return settingPrefabs.Find(x => x.GetSettingsType() == settingsType);
        }
    }
}