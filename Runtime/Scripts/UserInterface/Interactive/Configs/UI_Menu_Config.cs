using IbrahKit;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UI_Menu_Config
{
    [SerializeField] private UI_Menu_Button menuButtonPrefab;
    [SerializeField] private UI_Menu_Button staticMenuButtonPrefab;
    [SerializeField] private List<UI_Setting> settingPrefabs;

    public UI_Menu_Button GetMenuButton()
    {
        return menuButtonPrefab;
    }

    public UI_Menu_Button GetMenuButtonStatic()
    {
        return staticMenuButtonPrefab;
    }

    public UI_Setting GetSettingsPrefab(SettingsType settingsType)
    {
        return settingPrefabs.Find(x => x.GetSettingsType() == settingsType);
    }
}
