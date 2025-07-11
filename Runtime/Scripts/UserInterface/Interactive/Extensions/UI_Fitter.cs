using UnityEngine;
using Application = UnityEngine.Application;

namespace IbrahKit
{
    public abstract class UI_Fitter : UI_Extension
    {
        protected RectTransform rect;

        [SerializeField] protected UI_Fitter_Config_SO customConfig;

        [SerializeField] protected bool scaleWidth = true;
        [SerializeField] protected int maxWidth;
        [SerializeField] protected bool scaleHeight = true;
        [SerializeField] protected int maxHeight;
        [SerializeField] protected int heightOffset;

        protected override void Init()
        {
            if(rect == null && !TryGetComponent(out  rect))
            {
                return;
            }

            base.Init();
        }

        protected void SetSize(float size, float max, float offset, UI_Fitter_Config config, RectTransform.Axis axis)
        {
            float _max = Mathf.Clamp(size, 0, GetMax(maxHeight));
            rect.SetSizeWithCurrentAnchors(axis, _max + config.GetMargin() + offset);
        }

        protected float GetMax(float max)
        {
            return max == 0 ? Mathf.Infinity : max;
        }

        protected UI_Fitter_Config GetConfig()
        {
            if (customConfig != null)
            {
                return customConfig.GetConfig();
            }

            UI_Fitter_Config defaultConfig = null;

            if (!Application.isPlaying)
            {
                UI_Manager manager = FindFirstObjectByType<UI_Manager>();
                if (manager != null) defaultConfig = manager.GetDefaultUIConfig()?.GetConfig();
            }
            else
            {
                if (UI_Manager.Instance != null) defaultConfig = UI_Manager.Instance.GetDefaultUIConfig()?.GetConfig();
            }

            return defaultConfig ?? new UI_Fitter_Config(0);
        }

        protected RectTransform GetRect()
        {
            if (!Application.isPlaying)
            {
                return rect != null ? rect : GetComponent<RectTransform>();
            }
            else
            {
                return rect;
            }
        }

        public override int GetOrder()
        {
            return 100;
        }
    }
}