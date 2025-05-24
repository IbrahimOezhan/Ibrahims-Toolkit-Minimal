using UnityEngine;

namespace TemplateTools
{
    public class UI_Base : MonoBehaviour
    {
        [SerializeField] private UI_Menu_Basic parentMenu;

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void OnValidate()
        {
        }

        protected virtual void OnDestroy()
        {
        }

        public void SetParentMenu(UI_Menu_Basic menu)
        {
            parentMenu = menu;
        }

        public UI_Menu_Basic GetParentMenu()
        {
            return parentMenu;
        }
    }
}