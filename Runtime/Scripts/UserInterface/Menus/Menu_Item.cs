using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace IbrahKit
{
    [System.Serializable]
    public partial class Menu_Item
    {
        [SerializeReference] private Menu_Item_Base menuItem;

        [SerializeField] private bool skip;

        [SerializeField] private bool layoutSpecific;

        [ShowIf(nameof(layoutSpecific)), SerializeField, Dropdown(UI_Manager.UILAYOUTKEY)] private List<string> showOnLayouts;

        public GameObject Spawn(RectTransform parent, UI_Menu_Extended menu)
        {
            if (skip) return null;

            if (layoutSpecific && !UI_Manager.Instance.ShowLayout(showOnLayouts)) return null;

            menuItem.Spawn(parent, menu);

            return menuItem.GetSpawnedObject();
        }
    }
}