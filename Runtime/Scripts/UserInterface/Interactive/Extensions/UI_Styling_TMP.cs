using TMPro;
using UnityEngine;

namespace IbrahKit
{
    public class UI_Styling_TMP : UI_Styling
    {
        [SerializeField] private TextMeshProUGUI text;

        protected override void Init()
        {
            if (text == null && !TryGetComponent(out text))
            {
                return;
            }

            base.Init();
        }

        public override void Execute()
        {
            if (!init) Init();
            if (!init) return;

            TextMeshProUGUI _text = GetText();

            UI_Styling_Config _style = GetResolvedStyle((int)text.fontSize, text.color);

            (_text.font, _text.color) = _style.GetStyleTMP();
        }

        public override int GetOrder()
        {
            return 1;
        }

        public TextMeshProUGUI GetText()
        {
            return text;
        }
    }
}