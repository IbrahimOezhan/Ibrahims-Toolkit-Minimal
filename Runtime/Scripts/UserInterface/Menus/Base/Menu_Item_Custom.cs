using UnityEngine;
using UnityEngine.Events;

namespace IbrahKit
{
    [System.Serializable]
    public class Menu_Item_Custom : Menu_Item_Button
    {
        //[SerializeField] private UnityEvent unityEvent;

        public override void Spawn(RectTransform parent, UI_Menu_Extended menu)
        {
            //base.Spawn(parent, menu);
            //spawnedButton.Initialize(localizationKey).AddListener(() => { unityEvent.Invoke(); });
        }
    }
}