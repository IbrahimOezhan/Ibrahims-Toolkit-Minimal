using UnityEngine;

namespace TemplateTools
{
    public class Log
    {
        public static void LogNormal(string message, GameObject context = null)
        {
            if (Log_Manager.s_disableLogs) return;
            Debug.Log(message, context);
        }

        public static void LogWarning(string message, GameObject context = null)
        {
            if (Log_Manager.s_disableLogs) return;
            Debug.LogWarning(message, context);
        }

        public static void LogError(string message, GameObject context = null)
        {
            if (Log_Manager.s_disableLogs) return;
            Debug.LogError(message, context);
        }
    }
}