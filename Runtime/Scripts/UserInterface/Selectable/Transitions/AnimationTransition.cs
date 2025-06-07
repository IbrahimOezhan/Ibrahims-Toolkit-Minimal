using System;
using UnityEngine;
using static IbrahKit.UI_Selectable;

namespace IbrahKit
{
    [Serializable]
    public class AnimationTransition : SelectableTransition
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string none = "None";
        [SerializeField] private string hovering = "Hovering";
        [SerializeField] private string pressed = "Pressed";

        protected override void OnHovering(GameObject go)
        {
            animator.Play(hovering);
        }

        protected override void OnNone(GameObject go)
        {
            animator.Play(none);
        }

        protected override void OnPressed(GameObject go)
        {
            animator.Play(pressed);
        }
    }
}