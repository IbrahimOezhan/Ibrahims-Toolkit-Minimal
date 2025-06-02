using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IbrahKit
{
    [System.Serializable]
    public class UI_Style
    {
        public UI_Style(Font font, TMP_FontAsset fontAsset, int fontSize, Color fontColor)
        {
            this.TMP_Font = fontAsset;
            this.font = font;
            this.fontColor = fontColor;
        }

        public UI_Style()
        {

        }

        [SerializeField] private Font font;
        [SerializeField] private TMP_FontAsset TMP_Font;
        [SerializeField] private Color fontColor;

        [SerializeField] private List<ReplacementFont> replacementFonts = new();

        private class ReplacementFont
        {
            public Font font;
            public TMP_FontAsset TMP_Font;
            [Dropdown("SysLanguage")] public string language;
        }

        public (Font, Color) GetFont()
        {
            ReplacementFont font = replacementFonts.Find(x => Localization_Manager.Instance.GetCurrentSysLanguage().ToString() == x.language);

            if (font != null) return (font.font, fontColor);
            else return (this.font, fontColor);
        }


        public (TMP_FontAsset, Color) GetFontTMP()
        {
            ReplacementFont font = replacementFonts.Find(x => Localization_Manager.Instance.GetCurrentSysLanguage().ToString() == x.language);

            if (font != null) return (font.TMP_Font, fontColor);
            else return (this.TMP_Font, fontColor);
        }
    }
}