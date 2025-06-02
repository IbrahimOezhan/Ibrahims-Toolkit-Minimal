using UnityEngine;

namespace IbrahKit
{
    public class PlatformBased
    {
        public RuntimePlatform platform;

        public bool IsPlatform()
        {
            return (Application.platform == platform);
        }
    }
}