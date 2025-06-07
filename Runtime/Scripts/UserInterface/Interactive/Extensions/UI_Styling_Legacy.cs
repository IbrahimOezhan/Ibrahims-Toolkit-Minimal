using UnityEngine;
using UnityEngine.UI;

namespace IbrahKit
{
    public class UI_Styling_Legacy : UI_Styling
    {
        [SerializeField] private Text text;
        [SerializeField] private UI_Style_SO style;

        protected override void Init()
        {
            if (text == null)
            {
                if (!TryGetComponent(out text))
                {
                    return;
                }
            }
            base.Init();
        }

        public override void Execute()
        {
            if (!init) Init();

            Text _text = GetText();
            UI_Style _style = GetResolvedStyle(_text.fontSize, text.color);

            (_text.font, _text.color) = _style.GetStyle();
        }

        public override int GetOrder()
        {
            return 1;
        }

        public Text GetText()
        {
            return text != null ? text : GetComponent<Text>();
        }
    }
}