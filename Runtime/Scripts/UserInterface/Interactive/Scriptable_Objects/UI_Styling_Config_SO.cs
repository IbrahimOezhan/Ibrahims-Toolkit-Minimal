using UnityEngine;

namespace IbrahKit
{
    [CreateAssetMenu(fileName = "NewUIStyle", menuName = "IbrahKit/UI Style")]
    public class UI_Styling_Config_SO : ScriptableObject
    {
        [SerializeField] private UI_Styling_Config style = new();

        public UI_Styling_Config GetStyle()
        {
            return style;
        }
    }
}