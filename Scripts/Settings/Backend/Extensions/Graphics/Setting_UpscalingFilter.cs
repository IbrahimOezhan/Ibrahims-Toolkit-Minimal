using System;
using UnityEngine.Rendering.Universal;

namespace TemplateTools
{
    public class Setting_UpscalingFilter : Setting
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            maxValue = Enum.GetNames(typeof(UpscalingFilterSelection)).Length - 1;
        }

        public override void ApplyChanges()
        {
            base.ApplyChanges();
            Settings_Manager.Instance.pipelineAsset.upscalingFilter = (UpscalingFilterSelection)value;
        }

        public override string GetDisplayValue()
        {
            return Settings_Manager.Instance.pipelineAsset.upscalingFilter.ToString();
        }
    }
}