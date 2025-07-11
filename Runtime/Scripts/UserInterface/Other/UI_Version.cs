using UnityEngine;

namespace IbrahKit
{
    public class UI_Version : MonoBehaviour
    {
        [SerializeField] private UI_Localization localization;

        private void Awake()
        {
            if (localization == null && !TryGetComponent(out localization))
            {
                Debug.LogWarning($"No component of type {nameof(UI_Localization)} attached to the game object");
                return;
            }

            localization.SetParam(new() { Application.version });
        }
    }
}