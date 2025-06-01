using UnityEngine;

namespace TemplateTools
{
    [RequireComponent(typeof(UI_Interactive))]
    public abstract class UI_Extension : MonoBehaviour
    {
        protected bool init;

        private void Awake()
        {
            OnAwake();
        }

        private void OnDestroy()
        {
            OnOnDestroy();
        }

        protected virtual void OnOnDestroy()
        {

        }

        protected virtual void OnAwake()
        {
            Init();
        }

        protected virtual void Init()
        {
            init = true;
        }

        public virtual int GetOrder()
        {
            return 0;
        }

        public virtual void Execute()
        {

        }

        public void UpdateUI()
        {
            GetComponent<UI_Interactive>().UpdateUI();
        }
    }
}