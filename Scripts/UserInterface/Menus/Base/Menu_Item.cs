using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace IbrahKit
{
    [System.Serializable]
    public partial class Menu_Item
    {
        [TitleGroup("$menuType")] public Menu_Item_Type menuType;

        [TitleGroup("$menuType"), ShowIf("menuType", Menu_Item_Type.SETTING), SerializeField] private Menu_Item_Setting setting;

        [TitleGroup("$menuType"), ShowIf("menuType", Menu_Item_Type.MENUREF), SerializeField] private Menu_Item_Menu menu;

        [TitleGroup("$menuType"), ShowIf("menuType", Menu_Item_Type.CUSTOM), SerializeField] private Menu_Item_Custom1 custom;

        [TitleGroup("$menuType"), ShowIf("menuType", Menu_Item_Type.BACK), SerializeField] private Menu_Item_Back back = new();
        [TitleGroup("$menuType"), ShowIf("menuType", Menu_Item_Type.QUIT), SerializeField] private Menu_Item_Quit quit = new();

        [SerializeField] private bool skip;

        [SerializeField] private bool layoutSpecific;

        [ShowIf("layoutSpecific"), SerializeField] private List<int> showOnLayouts;

        public GameObject Spawn(RectTransform parent, UI_Menu_Extended menu)
        {
            if (skip) return null;

            if (layoutSpecific) if (!UI_Manager.Instance.ShowLayout(showOnLayouts)) return null;

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
                    menuItem = quit;
                    break;
            }

            menuItem.Spawn(parent, menu);
            return menuItem.GetSpawnedObject();
        }
    }
}