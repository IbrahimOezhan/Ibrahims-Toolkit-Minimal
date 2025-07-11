using Sirenix.OdinInspector;
using UnityEngine;

namespace IbrahKit
{
    [System.Serializable]
    public class UI_Menu_Transition
    {
        [SerializeField] private UI_Menu_Basic menu;
        [SerializeField] private FadeMode mode;
        [HideIf(nameof(mode), FadeMode.None)]
        [SerializeField] private float fadeTime;

        public (UI_Menu_Basic, FadeMode, float) GetData()
        {
            return (menu, mode,fadeTime);
        }
    }
}