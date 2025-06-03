using Sirenix.OdinInspector;

namespace IbrahKit
{
    [System.Serializable]
    public class MenuTransition
    {
        public UI_Menu_Basic menu;
        public FadeMode mode;
        [HideIf("mode", FadeMode.None)]
        public float fadeTime;
    }
}