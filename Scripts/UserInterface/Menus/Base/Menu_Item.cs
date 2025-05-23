using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

namespace TemplateTools
{
    [System.Serializable]
    public partial class Menu_Item
    {
        [OnValueChanged("OnTypeChanged"), TitleGroup("$menuType")] public Menu_Item_Type menuType;

        [TitleGroup("$menuType"), ShowIf("menuType", Menu_Item_Type.SETTING), SerializeField] private Menu_Item_Setting setting;

        [TitleGroup("$menuType"), ShowIf("menuType", Menu_Item_Type.MENUREF), SerializeField] private Menu_Item_Menu menu;

        [TitleGroup("$menuType"), ShowIf("menuType", Menu_Item_Type.CUSTOM), SerializeField] private Menu_Item_Custom1 custom;
        
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
    }
}