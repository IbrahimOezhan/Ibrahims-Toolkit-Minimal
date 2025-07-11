using System.Collections.Generic;
using UnityEngine;

namespace IbrahKit
{
    [System.Serializable]
    public class UI_Fitter_Config
    {
        [SerializeField] private float margin;

        [SerializeField] private List<PlatformBasedMargin> marginOverride = new();

        public UI_Fitter_Config(float margin)
        {
            this.margin = margin;
        }

        public float GetMargin()
        {
            if (marginOverride == null)
            {
                Debug.LogWarning($"{nameof(marginOverride)} is null");
                return margin;
            }

            for (int i = 0; i < marginOverride.Count; i++)
            {
                if (marginOverride[i].IsPlatform()) return marginOverride[i].margin;
            }

            return margin;
        }

        [System.Serializable]
        private class PlatformBasedMargin : PlatformBased
        {
            public float margin;
        }
    }
}