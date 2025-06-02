using System.Collections.Generic;
using UnityEngine;

namespace IbrahKit
{
    public class UI_PlatformHide : MonoBehaviour
    {
        [SerializeField] private List<RuntimePlatform> hide;

        private void Awake()
        {
            if (hide.Contains(Application.platform)) gameObject.SetActive(false);
        }

        public virtual bool HideCustom()
        {
            return false;
        }
    }
}