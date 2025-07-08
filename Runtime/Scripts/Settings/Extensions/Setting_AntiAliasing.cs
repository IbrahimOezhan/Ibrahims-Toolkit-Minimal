using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace IbrahKit
{
    public class Setting_AntiAliasing : Setting
    {
        private readonly int[] values = { 0, 2, 4, 8 };

        public override void ApplyChanges()
        {
            base.ApplyChanges();
            ((UniversalRenderPipelineAsset)GraphicsSettings.defaultRenderPipeline).msaaSampleCount = values[(int)GetValue()];
            //QualitySettings.antiAliasing = values[(int)value];
        }
    }
}