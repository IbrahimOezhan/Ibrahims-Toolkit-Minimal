using UnityEngine;

namespace IbrahKit
{
    public class Setting_VSync : Setting
    {
        public override void ApplyChanges()
        {
            base.ApplyChanges();
            QualitySettings.vSyncCount = (int)GetValue();
        }
    }

}