using System;
using System.Text;
using UnityEngine;

namespace TemplateTools
{
    public class Debug
    {
        private static StringBuilder buffer = new();

        public static void Log(object message, GameObject context = null)
        {
            if (IsDisabled()) return;

            if(Debug_Manager.bufferLogs)
            {
                string msg = "<color=white>" + message + "</color>";

                AddToBuffer(msg);

                return;
            }

            UnityEngine.Debug.Log(message, context);
        }

        public static void LogWarning(object message, GameObject context = null)
        {
            if (IsDisabled()) return;

            if (Debug_Manager.bufferLogs)
            {
                string msg = "<color=yellow>" + message + "</color>";

                AddToBuffer(msg);

                return;
            }

            UnityEngine.Debug.LogWarning(message, context);
        }

        public static void LogError(object message, GameObject context = null)
        {
            if (IsDisabled()) return;

            if (Debug_Manager.bufferLogs)
            {
                string msg = "<color=red>" + message + "</color>";

                AddToBuffer(msg);

                return;
            }

            UnityEngine.Debug.LogError(message, context);
        }

        private static void AddToBuffer(object msg)
        {
            buffer.AppendLine(msg.ToString());
        }

        public static void ReleaseBuffer()
        {
            UnityEngine.Debug.Log(buffer);
            buffer = new();
        }

        public static bool IsDisabled()
        {
            return Debug_Manager.s_disableLogs;
        }

        public static void Buffer(bool value)
        {
            Debug_Manager.bufferLogs = value;
            buffer = new();
        }
    }
}