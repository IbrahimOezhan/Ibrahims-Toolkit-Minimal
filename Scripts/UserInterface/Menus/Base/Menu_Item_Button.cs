using UnityEngine;

namespace TemplateTools
{
    [System.Serializable]
    public class Menu_Item_Button : Menu_Item_Base
    {
        protected UI_Menu_Button spawnedButton;

        [SerializeField, Dropdown("Localization")] protected string localizationKey;

        public override void Spawn(RectTransform parent, UI_Menu_Extended menu)
        {
            spawnedButton = Object.Instantiate(menu.GetMenuConfig().menuButtonPrefab, parent);
            spawnedObject = spawnedButton.gameObject;
        }
    }
}