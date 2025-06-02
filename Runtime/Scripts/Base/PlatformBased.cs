using UnityEngine;

namespace IbrahKit
{
    public class PlatformBased
    {
        [SerializeField] private RuntimePlatform platform;

        public bool IsPlatform()
        {
            return Application.platform == platform;
        }
    }
}