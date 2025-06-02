using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace IbrahKit
{
    public class UI_Localization_Legacy : UI_Localization
    {
        [SerializeField]
        private Text text;

        protected override void Init()
        {
            if (text == null) text = GetComponent<Text>();
            base.Init();
        }

        public override void Execute()
        {
            base.Execute();

            if (!init) Init();

            (Text text, Localization_Manager manager) = GetText();

            if (manager == null)
            {
                return;
            }

            text.text = manager.GetLocalizedString(key, fallbackText, parameters.Cast<object>().ToArray());
        }

        private (Text, Localization_Manager) GetText()
        {
            if (Application.isPlaying)
            {
                return (text, Localization_Manager.Instance);
            }
            else
            {
                Localization_Manager manager = FindFirstObjectByType<Localization_Manager>();

                if (manager == null)
                {
                    UnityEngine.Debug.LogWarning("No Localization_Manager found in scene.");
                    return (text, null);
                }

                return (text != null ? text : GetComponent<Text>(), manager);
            }
        }
    }
}