using UnityEngine;
using UnityEngine.UI;

namespace IbrahKit
{
    public class UI_Styling_Legacy : UI_Styling
    {
        [SerializeField] private Text text;
        [SerializeField] private UI_Styling_Config_SO style;

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

            Text _text = GetText();
            UI_Styling_Config _style = GetResolvedStyle(_text.fontSize, text.color);

            (_text.font, _text.color) = _style.GetStyle();
        }

        public override int GetOrder()
        {
            return 1;
        }

        public Text GetText()
        {
            return text;
        }
    }
}