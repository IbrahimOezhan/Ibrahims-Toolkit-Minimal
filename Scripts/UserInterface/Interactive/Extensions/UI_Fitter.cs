using UnityEngine;
using UnityEngine.UI;

namespace IbrahKit
{
    public class UI_Fitter : UI_Extension
    {
        private UI_Config defaultConfig;

        private RectTransform rect;

        [SerializeField] private Text text;
        [SerializeField] private UI_Config_So customConfig;

        [SerializeField] private bool scaleWidth = true;
        [SerializeField] private int maxWidth;
        [SerializeField] private bool scaleHeight = true;
        [SerializeField] private int maxHeight;
        [SerializeField] private int heightOffset;

        protected override void Init()
        {
            rect = GetComponent<RectTransform>();
            if (text == null) text = GetComponent<Text>();
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

            (Text text, RectTransform rect, UI_Config config) = GetRefs();

            if (scaleWidth)
            {
                float _maxWidth = Mathf.Clamp(text.preferredWidth, 0, maxWidth == 0 ? Mathf.Infinity : maxWidth);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _maxWidth + config.GetMargin());
            }
            if (scaleHeight)
            {
                float _maxHeight = Mathf.Clamp(text.preferredHeight, 0, maxHeight == 0 ? Mathf.Infinity : maxHeight);
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _maxHeight + config.GetMargin() + heightOffset);
            }
        }

        private (Text, RectTransform, UI_Config) GetRefs()
        {
            if (!Application.isPlaying)
            {
                UI_Manager manager = FindFirstObjectByType<UI_Manager>();

                Text text = this.text != null ? this.text : GetComponent<Text>();
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
                UI_Config config = null;

                if (customConfig != null) config = customConfig.config;
                else
                {
                    if (UI_Manager.Instance != null) config = UI_Manager.Instance.GetDefaultUIConfig().config;
                    else config = defaultConfig;
                }

                return (text, rect, config);
            }
        }
    }

}