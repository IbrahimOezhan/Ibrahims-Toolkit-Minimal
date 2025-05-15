using UnityEngine;

namespace TemplateTools
{
    public class Setting_VSync : Setting_KeyValue
    {
        public override void ApplyChanges()
        {
            base.ApplyChanges();
            QualitySettings.vSyncCount = (int)value;
        }
    }

}