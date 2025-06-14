using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IbrahKit
{
    [System.Serializable]
    public class UI_Styling_Config
    {
        public UI_Styling_Config()
        {

        }

        public UI_Styling_Config(Font font, TMP_FontAsset fontAsset, int fontSize, Color fontColor)
        {
            this.fontAsset = fontAsset;
            this.font = font;
            this.fontColor = fontColor;
        }

        [SerializeField] private Color fontColor;
        [SerializeField] private Font font;
        [SerializeField] private TMP_FontAsset fontAsset;
        [SerializeField] private List<ReplacementFont> replacementFonts = new();

        public (Font, Color) GetStyle()
        {
            ReplacementFont font = replacementFonts.Find(x => Localization_Manager.Instance.GetCurrentSysLanguage().ToString() == x.language);

            if (font != null) return (font.font, fontColor);
            else return (this.font, fontColor);
        }

        public (TMP_FontAsset, Color) GetStyleTMP()
        {
            ReplacementFont font = replacementFonts.Find(x => Localization_Manager.Instance.GetCurrentSysLanguage().ToString() == x.language);

            if (font != null) return (font.fontAsset, fontColor);
            else return (fontAsset, fontColor);
        }

        private class ReplacementFont
        {
            public Font font;
            public TMP_FontAsset fontAsset;
            [Dropdown("SysLanguage")] public string language;
        }
    }
}