using IbrahKit;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUILayoutConfig", menuName = "IbrahKit/UILayoutConfig")]
public class UI_Layout_Config : ScriptableObject
{
    private static UI_Layout_Config active;

    [SerializeField, OnValueChanged("OnValueChanged")] private List<string> layouts = new();

    private void OnValueChanged()
    {
        if (active == null) active = this;

        if (active == this)
        {
            List<string> list = new List<string>(layouts);
            list.Add("None");
            String_Utilities.CreateDropdown(list, "UILayouts");
        }
    }

    [Button(), ShowIf("@active != this")]
    private void SetActive()
    {
        active = this;
    }
}
