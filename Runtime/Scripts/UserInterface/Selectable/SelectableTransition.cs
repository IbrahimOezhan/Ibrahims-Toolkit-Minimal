using UnityEngine;

namespace IbrahKit
{
    [System.Serializable]
    public class SelectableTransition
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

        protected virtual void OnNone(GameObject go)
        {

        }
        protected virtual void OnHovering(GameObject go)
        {

        }
        protected virtual void OnPressed(GameObject go)
        {

        }
    }
}
