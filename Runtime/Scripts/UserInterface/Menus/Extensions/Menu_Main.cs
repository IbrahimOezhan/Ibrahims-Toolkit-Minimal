using UnityEngine;

namespace IbrahKit
{
    public class Menu_Main : UI_Menu_Extended
    {
        public virtual void StartGame() { }

        public void OpenSettings()
        {
            Settings_Manager.Instance.OpenSettings(this);
        }

        public void OpenURL(string _URL)
        {
            Application.OpenURL(_URL);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}