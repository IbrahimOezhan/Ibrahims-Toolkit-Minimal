using UnityEngine;

namespace TemplateTools
{
    public class Menu_Settings : MonoBehaviour
    {
        public static UI_Menu_Basic Instance;
        public UI_Menu_Basic menu;

        private void Awake()
        {
            Instance = menu;
        }
    }
}