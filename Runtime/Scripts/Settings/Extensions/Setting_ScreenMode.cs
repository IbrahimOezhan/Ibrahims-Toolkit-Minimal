using System;
using UnityEngine;

namespace IbrahKit
{
    public class Setting_ScreenMode : Setting
    {
        public override void Init(string initialValue)
        {
            base.Init(initialValue);
            SetValueRange(new(GetValueRange().x, Enum.GetNames(typeof(FullScreenMode)).Length - 1));
        }

        public override void ApplyChanges()
        {
            base.ApplyChanges();
            Screen.fullScreenMode = (FullScreenMode)GetValue();
        }

        public override string GetDisplayValue()
        {
            return Localization_Manager.Instance.GetLocalizedString(Screen.fullScreenMode.ToString());
        }
    }
}
