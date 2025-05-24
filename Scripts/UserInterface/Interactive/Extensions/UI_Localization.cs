using System.Collections.Generic;
using UnityEngine;

namespace TemplateTools
{
    public class UI_Localization : UI_Extension
    {
        [Dropdown("Localization"), SerializeField]
        protected string key;

        [SerializeField]
        protected string fallbackText;

        [SerializeField]
        protected List<string> parameters = new();

        protected override void OnAwake()
        {
            base.OnAwake();
            if (Localization_Manager.Instance != null) Localization_Manager.Instance.OnLanguageChanged += UpdateUI;
        }

        protected override void OnOnDestroy()
        {
            base.OnOnDestroy();
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