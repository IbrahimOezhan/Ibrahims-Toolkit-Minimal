using UnityEngine;

namespace IbrahKit
{
    public class Menu_Loading : UI_Menu_Basic
    {
        [SerializeField] private UI_Localization loadingText;

        public void SetLoadingKey(string _key)
        {
            loadingText.SetKey(_key);
            Enable(null);
        }
    }
}
