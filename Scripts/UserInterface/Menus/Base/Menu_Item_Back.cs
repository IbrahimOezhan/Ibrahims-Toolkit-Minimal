using UnityEngine;

namespace TemplateTools
{
    public partial class Menu_Item
    {
        public class Menu_Item_Back : Menu_Item_Button
        {
            public override void Spawn(RectTransform parent, UI_Menu_Extended menu)
            {
                base.Spawn(parent, menu);
                menu.Back();
            }
        }
    }
}