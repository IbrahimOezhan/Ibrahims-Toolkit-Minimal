using UnityEngine;

namespace IbrahKit
{
    public class UI_Version : MonoBehaviour
    {
        [SerializeField] private UI_Localization localization;

        private void Awake()
        {
            if (localization == null) localization = GetComponent<UI_Localization>();
            localization.SetParam(new() { Application.version });
        }
    }
}