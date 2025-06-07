using System;
using UnityEngine;
using static IbrahKit.UI_Selectable;

namespace IbrahKit
{
    [Serializable]
    public class ScaleTransition : SelectableTransition
    {
        [SerializeField] private RectTransform rect;
        [SerializeField] private float none;
        [SerializeField] private float hovering;
        [SerializeField] private float pressed;

        protected override void OnHovering(GameObject go)
        {
            if (rect == null) rect = go.GetComponent<RectTransform>();

            Vector3 scale = new();

            scale = new(hovering, hovering);
            rect.localScale = scale;
        }

        protected override void OnNone(GameObject go)
        {
            if (rect == null) rect = go.GetComponent<RectTransform>();

            Vector3 scale = new();

            scale = new(pressed, pressed);
            rect.localScale = scale;
        }

        protected override void OnPressed(GameObject go)
        {
            if (rect == null) rect = go.GetComponent<RectTransform>();

            Vector3 scale = new();

            scale = new(none, none);
            rect.localScale = scale;
        }
    }
}