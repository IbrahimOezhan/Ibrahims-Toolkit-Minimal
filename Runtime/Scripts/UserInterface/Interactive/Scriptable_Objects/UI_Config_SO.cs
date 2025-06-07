using UnityEngine;

namespace IbrahKit
{
    [CreateAssetMenu(fileName = "NewUIConfig", menuName = "IbrahKit/UI Config")]
    public class UI_Config_SO : ScriptableObject
    {
        [SerializeField] private UI_Config config;

        public UI_Config GetConfig()
        {
            return config;
        }
    }
}