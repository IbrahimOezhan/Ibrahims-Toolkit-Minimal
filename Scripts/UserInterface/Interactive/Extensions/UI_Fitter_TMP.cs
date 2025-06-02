using IbrahKit;
using TMPro;
using UnityEngine;

namespace IbrahKit
{
    public class UI_Fitter_TMP : UI_Extension
    {
        private UI_Config defaultConfig;

        private RectTransform rect;

        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private UI_Config_So customConfig;

        [SerializeField] private bool scaleWidth = true;
        [SerializeField] private int maxWidth;
        [SerializeField] private bool scaleHeight = true;
        [SerializeField] private int maxHeight;

        protected override void Init()
        {
            rect = GetComponent<RectTransform>();
            if (text == null) text = GetComponent<TextMeshProUGUI>();
            base.Init();
        }

        public override int GetOrder()
        {
            return 100;
        }

        public override void Execute()
        {
            base.Execute();

            if (!init) Init();

            (TextMeshProUGUI text, RectTransform rect, UI_Config config) = GetRefs();

            if (scaleWidth)
            {
                float _maxWidth = Mathf.Clamp(text.preferredWidth, 0, maxWidth == 0 ? Mathf.Infinity : maxWidth);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _maxWidth + config.GetMargin());
            }
            if (scaleHeight)
            {
                float _maxHeight = Mathf.Clamp(text.preferredHeight, 0, maxHeight == 0 ? Mathf.Infinity : maxHeight);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _maxHeight + config.GetMargin());
            }
        }

        private (TextMeshProUGUI, RectTransform, UI_Config) GetRefs()
        {
            if (!Application.isPlaying)
            {
                UI_Manager manager = FindFirstObjectByType<UI_Manager>();

                TextMeshProUGUI text = this.text != null ? this.text : GetComponent<TextMeshProUGUI>();
                RectTransform rect = this.rect != null ? this.rect : GetComponent<RectTransform>();

                defaultConfig = new(1);

                if (manager == null)
                {
                    return (text, rect, defaultConfig);
                }
                else
                {
                    return (text, rect, customConfig != null ? customConfig.config : manager.GetDefaultUIConfig().config != null ? manager.GetDefaultUIConfig().config : defaultConfig);
                }
            }
            else
            {
                return (text, rect, customConfig != null ? customConfig.config : UI_Manager.Instance.GetDefaultUIConfig().config != null ? UI_Manager.Instance.GetDefaultUIConfig().config : defaultConfig);
            }
        }
    }
}