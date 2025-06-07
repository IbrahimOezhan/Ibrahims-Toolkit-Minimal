using System;
using UnityEngine;
using UnityEngine.UI;
using static IbrahKit.UI_Selectable;

namespace IbrahKit
{
    [Serializable]
    public class ColorTransition : SelectableTransition
    {
        [SerializeField] private Graphic graphic;
        [SerializeField] private Color none;
        [SerializeField] private Color hovering;
        [SerializeField] private Color pressed;

        protected override void OnHovering(GameObject go)
        {
            Color c = new();
            c = hovering;
            graphic.color = c;
        }

        protected override void OnNone(GameObject go)
        {
            Color c = new();
            c = none;
            graphic.color = c;

        }

        protected override void OnPressed(GameObject go)
        {
            Color c = new();
            c = pressed;
            graphic.color = c;
        }
    }
}