using UnityEngine;

namespace IbrahKit
{
    [CreateAssetMenu(fileName = "NewUIConfig", menuName = "IbrahKit/UI Config")]
    public class UI_Fitter_Config_SO : ScriptableObject
    {
        [SerializeField] private UI_Fitter_Config config;

        public UI_Fitter_Config GetConfig()
        {
            return config;
        }
    }
}