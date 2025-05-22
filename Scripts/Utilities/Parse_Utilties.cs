namespace TemplateTools
{
    public class Parse_Utilties
    {
        public static float Parse(string input, float defaultValue = 0)
        {
            if (float.TryParse(input, out float result)) return result;
            else return defaultValue;
        }

        public static int Parse(string input, int defaultValue = 0)
        {
            if (int.TryParse(input, out int result)) return result;
            else return defaultValue;
        }

        public static bool Parse(string input, bool defaultValue = false)
        {
            if (bool.TryParse(input, out bool result)) return result;
            else return defaultValue;
        }

        public static bool ParseFloatToBool(float num)
        {
            return num != 0;
        }

        public static string ParseSecondsToMMSS(int _seconds)
        {
            short _minutes = 0;
            while (_seconds >= 60)
            {
                _minutes++;
                _seconds -= 60;
            }
            return _minutes.ToString("00") + ":" + _seconds.ToString("00");
        }
    }
}