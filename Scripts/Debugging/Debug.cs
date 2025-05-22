using UnityEngine;

namespace TemplateTools
{
    public class Debug
    {
        public static void Log(string message, GameObject context = null)
        {
            if (Debug_Manager.s_disableLogs) return;
            UnityEngine.Debug.Log(message, context);
        }

        public static void Log(Vector3 message, GameObject context = null)
        {
            if (Debug_Manager.s_disableLogs) return;
            UnityEngine.Debug.Log(message, context);
        }

        public static void LogWarning(string message, GameObject context = null)
        {
            if (Debug_Manager.s_disableLogs) return;
            UnityEngine.Debug.LogWarning(message, context);
        }

        public static void LogError(string message, GameObject context = null)
        {
            if (Debug_Manager.s_disableLogs) return;
            UnityEngine.Debug.LogError(message, context);
        }
    }
}