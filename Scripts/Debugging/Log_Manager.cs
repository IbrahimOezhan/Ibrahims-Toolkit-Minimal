using System.Collections.Generic;
using UnityEngine;

namespace TemplateTools
{
    [DefaultExecutionOrder(-10)]
    public class Log_Manager : Manager_Base
    {
        public static Log_Manager instance;

        public List<LogTag> tags = new List<LogTag>();

        public Dictionary<string, bool> tagsDisabled = new();

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;

                for (int i = 0; i < tags.Count; i++)
                {
                    tagsDisabled.Add(tags[i].tag, tags[i].disable);
                }
            }
        }

        public bool IsTagDisabled(string _tag)
        {
            if (tagsDisabled.TryGetValue(_tag, out bool result)) return result;
            return false;
        }
    }

    [System.Serializable]
    public class LogTag
    {
        public string tag;
        public bool disable;
    }
}