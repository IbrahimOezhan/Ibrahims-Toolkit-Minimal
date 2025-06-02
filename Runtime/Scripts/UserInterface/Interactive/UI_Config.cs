using System.Collections.Generic;
using UnityEngine;

namespace IbrahKit
{
    [System.Serializable]
    public class UI_Config
    {
        public UI_Config(float margin)
        {
            this.margin = margin;
        }

        [SerializeField] private float margin;

        [SerializeField] private List<PlatformBasedMargin> marginOverride = new();

        public float GetMargin()
        {
            for (int i = 0; i < marginOverride.Count; i++)
            {
                if (marginOverride[i].IsPlatform()) return marginOverride[i].margin;
            }

            return margin;
        }
    }
}