using UnityEngine;

namespace TemplateTools
{
    public class Setting_VSync : Setting
    {
        public override void ApplyChanges()
        {
            base.ApplyChanges();
            QualitySettings.vSyncCount = (int)value;
        }
    }

}