using UnityEngine;
using UnityEngine.SceneManagement;

namespace IbrahKit
{
    public class Menu_Pause : UI_Menu_Extended
    {
        public void MainMenu()
        {
            Menu_Pause_Instance.Instance.Pause();
            SceneManager.LoadScene(0);
        }

        public void OpenSettings()
        {
            Settings_Manager.Instance.OpenSettings(this);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}