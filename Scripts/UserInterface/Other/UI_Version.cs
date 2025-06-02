using UnityEngine;

namespace IbrahKit
{
    public class UI_Version : MonoBehaviour
    {
        [SerializeField] private UI_Localization localization;

        private void Awake()
        {
            if (localization == null)
            {
                if(!TryGetComponent(out localization))
                {
                    return;
                }
            }

            localization.SetParam(new() { Application.version });
        }
    }
}