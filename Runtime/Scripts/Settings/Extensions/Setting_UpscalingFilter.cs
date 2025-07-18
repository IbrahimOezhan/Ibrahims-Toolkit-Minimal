using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace IbrahKit
{
    public class Setting_UpscalingFilter : Setting
    {
        public override void Init(string initialValue)
        {
            base.Init(initialValue);
            SetValueRange(new(GetValueRange().x, Enum.GetNames(typeof(UpscalingFilterSelection)).Length - 1));
        }

        public override void ApplyChanges()
        {
            base.ApplyChanges();
            ((UniversalRenderPipelineAsset)GraphicsSettings.defaultRenderPipeline).upscalingFilter = (UpscalingFilterSelection)GetValue();
        }

        public override string GetDisplayValue()
        {
            return ((UniversalRenderPipelineAsset)GraphicsSettings.defaultRenderPipeline).upscalingFilter.ToString();
        }
    }
}