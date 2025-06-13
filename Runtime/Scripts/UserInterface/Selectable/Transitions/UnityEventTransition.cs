using UnityEngine;
using UnityEngine.Events;

namespace IbrahKit
{
    public class UnityEventTransition : SelectableTransition
    {
        [SerializeField] private UnityEvent OnNoneEvent;
        [SerializeField] private UnityEvent OnHoveringEvent;
        [SerializeField] private UnityEvent OnPressedEvent;

        protected override void OnHovering(GameObject go)
        {
            base.OnHovering(go);
            OnHoveringEvent.Invoke();
        }

        protected override void OnNone(GameObject go)
        {
            base.OnNone(go);
            OnNoneEvent.Invoke();
        }

        protected override void OnPressed(GameObject go)
        {
            base.OnPressed(go);
            OnPressedEvent.Invoke();
        }
    }
}