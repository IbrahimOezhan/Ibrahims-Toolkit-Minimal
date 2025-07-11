using UnityEngine.EventSystems;

namespace IbrahKit
{
    public class UI_Audio : UI_Extension, IPointerDownHandler, IPointerEnterHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            if (UI_Manager.Exists(out UI_Manager um, true))
            {
                um.OnUIClick();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (UI_Manager.Exists(out UI_Manager um, true))
            {
                um.OnUIHover();
            }
        }
    }
}