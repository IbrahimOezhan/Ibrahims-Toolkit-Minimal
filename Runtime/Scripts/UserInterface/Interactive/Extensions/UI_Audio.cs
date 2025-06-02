using UnityEngine.EventSystems;

namespace IbrahKit
{
    public class UI_Audio : UI_Extension, IPointerDownHandler, IPointerEnterHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            UI_Manager.Instance.OnUIClick();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UI_Manager.Instance.OnUIHover();
        }
    }
}