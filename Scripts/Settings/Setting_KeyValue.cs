using UnityEngine;

namespace TemplateTools
{
    public class Setting_KeyValue : Setting
    {
        [Dropdown("Localization")][SerializeField] protected string[] keys;

        protected override void OnAwake()
        {
            base.OnAwake();
            maxValue = keys.Length - 1;
        }

        public override string GetDisplayValue()
        {
            return Localization_Manager.Instance.GetLocalizedString(keys[(int)(value / steps)], "");
        }
    }
}