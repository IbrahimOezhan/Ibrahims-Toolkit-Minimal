using UnityEngine;
using UnityEngine.UI;

namespace IbrahKit
{
    public class UI_Styling_Legacy : UI_Extension
    {
        private UI_Style defaultStyle;

        [SerializeField] private Text text;
        [SerializeField] private UI_Style_SO style;

        protected override void Init()
        {
            if (text == null) text = GetComponent<Text>();
            base.Init();
        }

        public override void Execute()
        {
            if (!init) Init();

            (Text _text, UI_Style _style) = GetTextAndStyle();

            (Font font, Color color) = _style.GetFont();

            _text.font = font;
            _text.color = color;
        }

        public override int GetOrder()
        {
            return 1;
        }

        public (Text, UI_Style) GetTextAndStyle()
        {
            if (!Application.isPlaying)
            {
                UI_Manager manager = FindFirstObjectByType<UI_Manager>();

                Text text = this.text != null ? this.text : GetComponent<Text>();

                defaultStyle = new(Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"), null, text.fontSize, text.color);

                if (manager == null)
                {
                    return (text, defaultStyle);
                }
                else
                {
                    return (text, style != null ? style.style : manager.GetDefaultStyle() != null ? manager.GetDefaultStyle().style : defaultStyle);
                }
            }
            else
            {
                return (text, style != null ? style.style : UI_Manager.Instance.GetDefaultStyle() != null ? UI_Manager.Instance.GetDefaultStyle().style : defaultStyle);
            }
        }
    }
}