using UnityEngine;

namespace IbrahKit
{
    [CreateAssetMenu(fileName = "NewUIMenuConfig", menuName = "IbrahKit/UI MenuConfig")]
    public class UI_Menu_Config_SO : ScriptableObject
    {
        [SerializeField] private UI_Menu_Config config;

        public UI_Menu_Config GetConfig()
        {
            return config;
        }
    }
}