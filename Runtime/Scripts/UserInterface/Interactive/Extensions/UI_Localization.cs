using System.Collections.Generic;
using UnityEngine;

namespace IbrahKit
{
    public abstract class UI_Localization : UI_Extension
    {
        [Dropdown("Localization"), SerializeField]
        protected string key;

        [SerializeField]
        protected string fallbackText;

        [SerializeField]
        protected List<string> parameters = new();

        protected override void Awake()
        {
            base.Awake();
            if (Localization_Manager.Instance != null) Localization_Manager.Instance.OnLanguageChanged += UpdateUI;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Localization_Manager.Instance != null) Localization_Manager.Instance.OnLanguageChanged -= UpdateUI;
        }

        public void SetFallback(string _fallback)
        {
            fallbackText = _fallback;
            UpdateUI();
        }

        public void SetKey(string _key)
        {
            key = _key;
            UpdateUI();
        }

        public void SetParam(List<string> _params)
        {
            parameters = _params;
            UpdateUI();
        }

        public void SetKeyParam(string _key, List<string> _params)
        {
            key = _key;
            parameters = _params;
            UpdateUI();
        }
    }
}