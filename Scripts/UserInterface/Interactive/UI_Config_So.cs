using UnityEngine;

namespace IbrahKit
{
    [CreateAssetMenu(fileName = "NewUIConfig", menuName = "ScriptableObjects/UI_Config_so")]
    public class UI_Config_So : ScriptableObject
    {
        [SerializeField] private UI_Config config;

        public UI_Config GetConfig()
        {
            return config;
        }
    }
}