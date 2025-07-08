using UnityEngine;

namespace IbrahKit
{
    public class Setting_FPS_Limit : Setting
    {
        [SerializeField] private readonly int[] targets = { -1, 10, 30, 60, 75, 144, 165, 360 };
        [SerializeField] private string[] values = { "Off", "10fps", "30fps", "60fps", "75fps", "144fps", "165fps", "360fps" };

        public override void ApplyChanges()
        {
            base.ApplyChanges();
            Application.targetFrameRate = targets[(int)GetValue()];
        }

        public override string GetDisplayValue()
        {
            return values[(int)GetValue()];
        }
    }
}