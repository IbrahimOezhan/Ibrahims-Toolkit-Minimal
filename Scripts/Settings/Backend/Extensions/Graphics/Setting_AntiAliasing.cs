using UnityEngine;

namespace TemplateTools
{
    public class Setting_AntiAliasing : Setting_KeyValue
    {
        private readonly int[] values = { 0, 2, 4, 8 };

        public override void ApplyChanges()
        {
            base.ApplyChanges();
            QualitySettings.antiAliasing = values[(int)value];
        }
    }
}