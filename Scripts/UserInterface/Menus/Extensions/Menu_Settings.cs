using UnityEngine;

namespace TemplateTools
{
    public class Menu_Settings : MonoBehaviour
    {
        [SerializeField] private UI_Menu_Basic menu;

        public static UI_Menu_Basic Instance;

        private void Awake()
        {
            Instance = menu;
        }
    }
}