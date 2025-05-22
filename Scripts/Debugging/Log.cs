using UnityEngine;

namespace TemplateTools
{
    public class Log
    {
        public static void SendLog(string _title, string _tag, string _value)
        {
            if (Log_Manager.instance && Log_Manager.instance.IsTagDisabled(_tag)) return;

            Debug.Log("<color=cyan>" + _tag + "</color> " + _title + " " + _value);
        }
    }
}