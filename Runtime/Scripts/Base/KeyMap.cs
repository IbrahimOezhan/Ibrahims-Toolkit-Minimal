using UnityEngine;
using UnityEngine.InputSystem;

namespace IbrahKit
{
    [CreateAssetMenu(fileName = "NewKeyMap", menuName = "IbrahKit/Keymap")]
    public class KeyMap : ScriptableObject
    {
        const string format = "Open/Close Debug Menu: {0}\nHide/Unhide UI: {1}\nScreenshot: {2}\nScreenshot without UI: {3}";

        public Key debugMenu;
        public Key hideUI;
        public Key screenshot;
        public Key screenshotNoUI;


        public override string ToString()
        {
            return string.Format(format, debugMenu.ToString(), hideUI.ToString(), screenshot.ToString(), screenshotNoUI.ToString());
        }
    }
}