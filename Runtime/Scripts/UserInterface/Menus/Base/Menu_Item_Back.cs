using UnityEngine;

namespace IbrahKit
{
    [System.Serializable]
    public class Menu_Item_Back : Menu_Item_Button
    {
        public override void Spawn(RectTransform parent, UI_Menu_Extended menu)
        {
            base.Spawn(parent, menu);
            spawnedButton.Initialize(localizationKey).AddListener(() =>
            {
                menu.Back();
            });
        }
    }
}