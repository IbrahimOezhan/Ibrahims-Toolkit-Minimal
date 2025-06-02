using System;
using UnityEngine;

namespace IbrahKit
{
    public class Setting_ScreenMode : Setting
    {
        protected override void Init()
        {
            base.Init();
            maxValue = Enum.GetNames(typeof(FullScreenMode)).Length - 1;
        }

        public override void ApplyChanges()
        {
            base.ApplyChanges();
            Screen.fullScreenMode = (FullScreenMode)value;
        }

        public override string GetDisplayValue()
        {
            return Localization_Manager.Instance.GetLocalizedString(Screen.fullScreenMode.ToString());
        }
    }
}
