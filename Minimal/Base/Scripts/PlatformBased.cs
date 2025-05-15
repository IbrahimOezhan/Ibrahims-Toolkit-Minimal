using UnityEngine;

namespace TemplateTools
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