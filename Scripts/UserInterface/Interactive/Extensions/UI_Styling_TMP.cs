using TMPro;
using UnityEngine;

namespace IbrahKit
{
    public class UI_Styling_TMP : UI_Extension
    {
        private UI_Style defaultStyle;

        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private UI_Style_SO style;

        protected override void Init()
        {
            if (text == null)
            {
                if(!TryGetComponent(out text))
                {
                    return;
                }
            }

            base.Init();
        }

        public override void Execute()
        {
            if (!init) Init();

            (TextMeshProUGUI _text, UI_Style _style) = GetTextAndStyle();
            (TMP_FontAsset font, Color color) = _style.GetFontTMP();

            _text.font = font;
            _text.color = color;
        }

        public override int GetOrder()
        {
            return 1;
        }

        public (TextMeshProUGUI, UI_Style) GetTextAndStyle()
        {
            if (!Application.isPlaying)
            {
                UI_Manager manager = FindFirstObjectByType<UI_Manager>();

                TextMeshProUGUI text = this.text != null ? this.text : GetComponent<TextMeshProUGUI>();

                defaultStyle = new(null, TMP_Settings.defaultFontAsset, (int)text.fontSize, text.color);

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
                UI_Manager manager = UI_Manager.Instance;

                return (text, style != null ? style.style : manager.GetDefaultStyle() != null ? manager.GetDefaultStyle().style : defaultStyle);
            }
        }
    }
}