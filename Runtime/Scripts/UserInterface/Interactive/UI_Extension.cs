using UnityEngine;

namespace IbrahKit
{
    [RequireComponent(typeof(UI_Interactive))]
    [AddComponentMenu("")]
    public abstract class UI_Extension : MonoBehaviour
    {
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
            init = true;
        }

        public virtual void Execute()
        {

        }

        public void UpdateUI()
        {
            GetComponent<UI_Interactive>().UpdateUI();
        }

        public virtual int GetOrder()
        {
            return 0;
        }
    }
}