using TMPro;
using UnityEngine;

namespace IbrahKit
{
    public abstract class UI_Styling : UI_Extension
    {
        [SerializeField] private UI_Style_SO customStyle;

        public UI_Style GetResolvedStyle(int defaultSize, Color defaultColor)
        {
            UI_Style defaultStyle = new UI_Style(
                Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"),
                TMP_Settings.defaultFontAsset, defaultSize, defaultColor
            );

            UI_Style resolvedStyle = null;

            if (!Application.isPlaying)
            {
                UI_Manager manager = FindFirstObjectByType<UI_Manager>();

                if (customStyle != null)
                {
                    resolvedStyle = customStyle.GetStyle();
                }
                else if (manager != null && manager.GetDefaultStyle() != null)
                {
                    resolvedStyle = manager.GetDefaultStyle().GetStyle();
                }
                else
                {
                    resolvedStyle = defaultStyle;
                }
            }
            else
            {
                if (customStyle != null)
                {
                    resolvedStyle = customStyle.GetStyle();
                }
                else if (UI_Manager.Instance != null && UI_Manager.Instance.GetDefaultStyle() != null)
                {
                    resolvedStyle = UI_Manager.Instance.GetDefaultStyle().GetStyle();
                }
                else
                {
                    resolvedStyle = defaultStyle;
                }
            }

            return resolvedStyle;
        }
    }
}
