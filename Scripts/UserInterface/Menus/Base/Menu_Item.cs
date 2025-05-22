using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TemplateTools
{
    [System.Serializable]
    public class Menu_Item
    {
        [OnValueChanged("OnTypeChanged"), TitleGroup("$menuType")] public Menu_Item_Type menuType;

        [TitleGroup("$menuType"), ShowIf("requiresLocalization"), Dropdown("Localization")] public string localizationKey;

        [TitleGroup("$menuType"), ShowIf("menuType", Menu_Item_Type.SETTING)] public Menu_Item_Setting setting;

        [TitleGroup("$menuType"), ShowIf("menuType", Menu_Item_Type.MENUREF)] public Menu_Item_Menu menu;

        [TitleGroup("$menuType"), ShowIf("menuType", Menu_Item_Type.CUSTOM)] public UnityEvent customEvent;

        public bool skip;

        public bool layoutSpecific;

        [ShowIf("layoutSpecific")] public int layout;

        [HideInInspector] public bool requiresLocalization;

        public void OnTypeChanged()
        {
            requiresLocalization = !menuType.Equals(Menu_Item_Type.SETTING);
        }


        [System.Serializable]
        public class Menu_Item_Setting
        {
            public SettingsInterfaceType settingType;

            [ShowIf("settingType", SettingsInterfaceType.REFERENCE)]
            public Setting reference;

            [ShowIf("settingType", SettingsInterfaceType.KEY), Dropdown("Settings")]
            public string settingsKey;
        }

        [System.Serializable]
        public class Menu_Item_Menu
        {
            [SerializeField] private Menu_Change_Type changeType;

            [ShowIf("changeType", Menu_Change_Type.REFERENCE), SerializeField] private UI_Menu_Basic menuReference;
            [ShowIf("changeType", Menu_Change_Type.TRANSITION), SerializeField] private int transitionReference = -1;

            public (UI_Menu_Basic, int) GetMenu()
            {
                switch (changeType)
                {
                    case Menu_Change_Type.REFERENCE:
                        if (menuReference == null) throw new NullReferenceException("Menu reference not set");
                        return (menuReference, -1);
                    case Menu_Change_Type.TRANSITION:
                        if (transitionReference == -1) throw new ArgumentOutOfRangeException("Transition reference not valid");
                        return (null, transitionReference);
                }

                return (null, -1);
            }

            public enum Menu_Change_Type
            {
                REFERENCE,
                TRANSITION,
            }
        }
    }
}