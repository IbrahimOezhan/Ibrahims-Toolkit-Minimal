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

        [ShowIf("layoutSpecific"), SerializeField] private List<int> showOnLayouts;

        public GameObject Spawn(RectTransform parent, UI_Menu_Extended menu)
        {
            if (skip) return null;

            if (layoutSpecific) if (!UI_Manager.Instance.ShowLayout(showOnLayouts)) return null;

            menuItem.Spawn(parent, menu);

            return menuItem.GetSpawnedObject();
        }
    }
}