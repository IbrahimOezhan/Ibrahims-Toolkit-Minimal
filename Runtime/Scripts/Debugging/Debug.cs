using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace IbrahKit
{
    public static class Debug
    {
        private static StringBuilder buffer = new();

        public static bool BufferLogs { get; private set; } = false;

        public static bool DisableLogs { get; set; } = false;

        private static void AddToBuffer(string message)
        {
            buffer.AppendLine(message);
        }

        public static void ReleaseBuffer()
        {
            if (buffer.Length > 0)
            {
                UnityEngine.Debug.Log(buffer.ToString());
                buffer.Clear();
            }
        }

        public static void SetBuffering(bool enable)
        {
            BufferLogs = enable;
            if (!enable)
                ReleaseBuffer();
            else
                buffer.Clear();
        }

        public static void Log(object message, UnityEngine.Object context = null,
            [CallerMemberName] string caller = null)
        {
            if (DisableLogs) return;

            string formattedMsg = $"[Log] {message} (Caller: {caller})";

            if (BufferLogs)
            {
                AddToBuffer(formattedMsg);
                return;
            }

            if (context != null)
                UnityEngine.Debug.Log(formattedMsg, context);
            else
                UnityEngine.Debug.Log(formattedMsg);
        }

        public static void LogWarning(object message, UnityEngine.Object context = null,
            [CallerMemberName] string caller = null)
        {
            if (DisableLogs) return;

            string formattedMsg = $"<color=yellow>[Warning] {message} (Caller: {caller})</color>";

            if (BufferLogs)
            {
                AddToBuffer(formattedMsg);
                return;
            }

            if (context != null)
                UnityEngine.Debug.LogWarning(formattedMsg, context);
            else
                UnityEngine.Debug.LogWarning(formattedMsg);
        }

        public static void LogError(object message, UnityEngine.Object context = null,
            [CallerMemberName] string caller = null)
        {
            if (DisableLogs) return;

            string formattedMsg = $"<color=red>[Error] {message} (Caller: {caller})</color>";

            if (BufferLogs)
            {
                AddToBuffer(formattedMsg);
                return;
            }

            if (context != null)
                UnityEngine.Debug.LogError(formattedMsg, context);
            else
                UnityEngine.Debug.LogError(formattedMsg);
        }

        public static void LogException(Exception exception, UnityEngine.Object context = null)
        {
            if (DisableLogs) return;

            if (BufferLogs)
            {
                AddToBuffer($"<color=red>[Exception] {exception}</color>");
                return;
            }

            if (context != null)
                UnityEngine.Debug.LogException(exception, context);
            else
                UnityEngine.Debug.LogException(exception);
        }
    }
}
