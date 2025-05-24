using UnityEngine;

namespace TemplateTools
{
    [System.Serializable]
    public class Menu_Item_Custom : Menu_Item
    {
        [SerializeField] private Vector2 position;
        [SerializeField] private Vector2 xAnchor;
        [SerializeField] private Vector2 yAnchor;
        [SerializeField] private Vector2 pivot;

        public void SetRectTransform(RectTransform rectTransform)
        {
            rectTransform.localPosition = position;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.anchorMin = new(xAnchor.x, yAnchor.x);
            rectTransform.anchorMax = new(xAnchor.y, yAnchor.y);
            rectTransform.pivot = pivot;
        }
    }
}

