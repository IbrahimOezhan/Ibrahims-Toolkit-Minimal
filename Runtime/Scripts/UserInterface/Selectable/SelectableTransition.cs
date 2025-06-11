using UnityEngine;

namespace IbrahKit
{
    public abstract class SelectableTransition
    {
        public void Apply(SelectedState state, GameObject go)
        {
            switch (state)
            {
                case SelectedState.None: OnNone(go); break;
                case SelectedState.Hovering: OnHovering(go); break;
                case SelectedState.Pressed: OnPressed(go); break;
            }
        }

        protected abstract void OnNone(GameObject go);
        protected abstract void OnHovering(GameObject go);
        protected abstract void OnPressed(GameObject go);
    }
}
