using UnityEngine;

namespace IbrahKit
{
    [RequireComponent(typeof(UI_Interactive))]
    [AddComponentMenu("")]
    public abstract class UI_Extension : MonoBehaviour
    {
        private UI_Interactive uiInteractive;

        protected bool init;

        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void OnDestroy()
        {

        }

        protected virtual void Init()
        {
            if (uiInteractive == null)
            {
                if (!TryGetComponent(out uiInteractive))
                {
                    return;
                }
            }

            init = true;
        }

        public virtual void Execute()
        {

        }

        public void UpdateUI()
        {
            if (!init) Init();
            if (!init) return;

            uiInteractive.UpdateUI();
        }

        public virtual int GetOrder()
        {
            return 0;
        }
    }
}