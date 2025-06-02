using System.Linq;
using TMPro;
using UnityEngine;

namespace IbrahKit
{
    public class UI_Localization_TMP : UI_Localization
    {
        [SerializeField]
        private TextMeshProUGUI text;

        protected override void Init()
        {
            if (text == null) text = GetComponent<TextMeshProUGUI>();
            base.Init();
        }

        public override void Execute()
        {
            base.Execute();

            if (!init) Init();

            (TextMeshProUGUI text, Localization_Manager manager) = GetText();

            text.text = manager.GetLocalizedString(key, fallbackText, parameters.Cast<object>().ToArray());
        }

        private (TextMeshProUGUI, Localization_Manager) GetText()
        {
            if (Application.isPlaying)
            {
                return (text, Localization_Manager.Instance);
            }
            else
            {
                return (text != null ? text : GetComponent<TextMeshProUGUI>(), FindFirstObjectByType<Localization_Manager>());
            }
        }
    }
}