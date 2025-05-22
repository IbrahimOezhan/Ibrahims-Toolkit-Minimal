using System.Collections.Generic;
using UnityEngine;

namespace TemplateTools
{
    public class Color_Utilities
    {
        public static bool ColorDifference(Color color1, Color color2, float tolerance)
        {
            if (Mathf.Abs(color1.r - color2.r) > tolerance) return true;
            if (Mathf.Abs(color1.b - color2.b) > tolerance) return true;
            if (Mathf.Abs(color1.g - color2.g) > tolerance) return true;
            return false;
        }

        public static Color ColorBlend(List<Color> colors)
        {
            if (colors.Count == 0) return Color.white;
            if (colors.Count == 1) return colors[0];

            Color newCol = colors[0];

            for (int i = 1; i < colors.Count; i++)
            {
                newCol = Color.Lerp(newCol, colors[i], .5f);
            }

            return newCol;
        }

        public static float ColorLuminance(Color color)
        {
            return 0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b;
        }

        public static Color InvertColor(Color color)
        {
            return new Color(1 - color.r, 1 - color.g, 1 - color.b, color.a);
        }

        public static float RGBverage(Color color)
        {
            return (color.r + color.g + color.b) / 3;
        }
    }
}