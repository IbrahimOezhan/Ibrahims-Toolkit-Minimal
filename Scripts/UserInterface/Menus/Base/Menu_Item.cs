using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;
using UnityEngine.UI;

namespace TemplateTools
{
    [System.Serializable]
    public partial class Menu_Item
    {
        [OnValueChanged("OnTypeChanged"), TitleGroup("$menuType")] public Menu_Item_Type menuType;

        [TitleGroup("$menuType"), ShowIf("menuType", Menu_Item_Type.SETTING), SerializeField] private Menu_Item_Setting setting;

        [TitleGroup("$menuType"), ShowIf("menuType", Menu_Item_Type.MENUREF), SerializeField] private Menu_Item_Menu menu;

        [TitleGroup("$menuType"), ShowIf("menuType", Menu_Item_Type.CUSTOM), SerializeField] private Menu_Item_Custom custom;
        
        private Menu_Item_Back back;

        public bool skip;

        public bool layoutSpecific;

        [ShowIf("layoutSpecific")] public int layout;

        public GameObject Spawn(RectTransform parent, UI_Menu_Extended menu)
        {
            Menu_Item_Base menuItem = null;

            switch (menuType)
            {
                case Menu_Item_Type.SETTING:
                    menuItem = setting;                    
                    break;
                case Menu_Item_Type.MENUREF:
                    menuItem = this.menu;
                    break;
                case Menu_Item_Type.CUSTOM:
                    menuItem = custom;
                    break;
                case Menu_Item_Type.BACK:
                    menuItem = back;
                    break;
                case Menu_Item_Type.QUIT:
                    break;
            }

            menuItem.Spawn(parent, menu);
            return menuItem.GetSpawnedObject();
        }

        public abstract class Menu_Item_Base
        {
            [SerializeField] protected GameObject spawnedObject;

            public GameObject GetSpawnedObject()
            {
                return   spawnedObject;
            }

            public abstract void Spawn(RectTransform parent, UI_Menu_Extended menu);
        }

        [System.Serializable]
        public class Menu_Item_Setting : Menu_Item_Base
        {
            public SettingsInterfaceType settingType;

            [ShowIf("settingType", SettingsInterfaceType.REFERENCE)]
            public Setting reference;

            [ShowIf("settingType", SettingsInterfaceType.KEY), Dropdown("Settings")]
            public string settingsKey;

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

                switch(settingType)
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

        public class Menu_Item_Custom : MenuButton_Item
        {
            [SerializeField] private UnityEvent unityEvent;

            public override void Spawn(RectTransform parent, UI_Menu_Extended menu)
            {
                base.Spawn(parent, menu);
                unityEvent.Invoke();
            }
        }

        public class Menu_Item_Back : MenuButton_Item
        {
            public override void Spawn(RectTransform parent, UI_Menu_Extended menu)
            {
                base.Spawn(parent, menu);
                menu.Back();
            }
        }

        [System.Serializable]
        public class Menu_Item_Menu : MenuButton_Item
        {
            [SerializeField] private Menu_Change_Type changeType;

            [ShowIf("changeType", Menu_Change_Type.REFERENCE), SerializeField] private UI_Menu_Basic menuReference;
            [ShowIf("changeType", Menu_Change_Type.TRANSITION), SerializeField] private int transitionReference = -1;

            public override void Spawn(RectTransform parent, UI_Menu_Extended menu)
            {
                base.Spawn(parent, menu);

                switch (changeType)
                {
                    case Menu_Change_Type.REFERENCE:
                        spawnedButton.Initialize(localizationKey).AddListener(() =>
                        {
                            menu.MenuTransition(menuReference);
                        });
                        break;
                    case Menu_Change_Type.TRANSITION:
                        spawnedButton.Initialize(localizationKey).AddListener(() =>
                        {
                            menu.MenuTransition(transitionReference);
                        });
                        break;
                }    
            }

            public enum Menu_Change_Type
            {
                REFERENCE,
                TRANSITION,
            }
        }
    }
}