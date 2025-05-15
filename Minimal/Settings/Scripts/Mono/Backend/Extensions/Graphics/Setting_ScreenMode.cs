using System;
using UnityEngine;

namespace TemplateTools
{
    public class Setting_ScreenMode : Setting
    {
        protected override void OnAwake()
        {
            base.OnAwake();
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
