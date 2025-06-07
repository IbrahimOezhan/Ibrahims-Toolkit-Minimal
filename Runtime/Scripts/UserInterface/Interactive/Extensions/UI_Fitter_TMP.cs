using TMPro;
using UnityEngine;

namespace IbrahKit
{
    public class UI_Fitter_TMP : UI_Fitter
    {
        [SerializeField] private TextMeshProUGUI text;

        protected override void Init()
        {
            if (text == null) text = GetComponent<TextMeshProUGUI>();
            base.Init();
        }

        public override void Execute()
        {
            base.Execute();

            if (!init) Init();

            (TextMeshProUGUI text, RectTransform rect, UI_Config config) = (GetText(), GetRect(), GetConfig());

            if (scaleWidth) SetSize(text.preferredWidth, maxWidth, 0, config, RectTransform.Axis.Horizontal);
            if (scaleHeight) SetSize(text.preferredHeight, maxHeight, heightOffset, config, RectTransform.Axis.Vertical);
        }

        private TextMeshProUGUI GetText()
        {
            if (!Application.isPlaying)
            {
                return this.text != null ? this.text : GetComponent<TextMeshProUGUI>();
            }
            else
            {
                return this.text;
            }
        }
    }
}