using UnityEngine;

namespace TemplateTools
{
    [DefaultExecutionOrder(-10)]
    public class Log_Manager : Manager_Base
    {
        public bool disableLogs;
        public static bool s_disableLogs;
        public static Log_Manager instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void Update()
        {
            s_disableLogs = disableLogs;
        }
    }
}