using System.Text;
using UnityEngine;

namespace TemplateTools
{
    public class Debug
    {
        private static StringBuilder buffer = new();

        public static void Log(string message, GameObject context = null)
        {
            if (IsDisabled()) return;

            if(Debug_Manager.bufferLogs)
            {
                buffer.AppendLine("<color=white>" + message + "</color>");
                return;
            }

            UnityEngine.Debug.Log(message, context);
        }

        public static void LogWarning(string message, GameObject context = null)
        {
            if (IsDisabled()) return;

            if (Debug_Manager.bufferLogs)
            {
                buffer.AppendLine("<color=yellow>" + message + "</color>");
                return;
            }

            UnityEngine.Debug.LogWarning(message, context);
        }

        public static void LogError(string message, GameObject context = null)
        {
            if (IsDisabled()) return;

            if (Debug_Manager.bufferLogs)
            {
                buffer.AppendLine("<color=red>" + message + "</color>");
                return;
            }

            UnityEngine.Debug.LogError(message, context);
        }

        public static void ReleaseBuffer()
        {
            UnityEngine.Debug.Log(buffer.ToString());
            buffer.Clear();
        }

        public static bool IsDisabled()
        {
            return Debug_Manager.s_disableLogs;
        }
    }
}