using UnityEngine;

namespace IbrahKit
{
    [CreateAssetMenu(fileName = "NewUIStyle", menuName = "IbrahKit/UI Style")]
    public class UI_Style_SO : ScriptableObject
    {
        [SerializeField] private UI_Style style = new();

        public UI_Style GetStyle()
        {
            return style;
        }
    }
}