using System.Collections.Generic;
using TemplateTools;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUIMenuConfig", menuName = "ScriptableObjects/UI_MenuConfig")]
public class UI_Menu_Config : ScriptableObject
{
    public UI_Menu_Button menuButtonPrefab;

    public List<UI_Setting> settingPrefabs;
}
