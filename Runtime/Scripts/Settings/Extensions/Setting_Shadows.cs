using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace IbrahKit
{
    public class Setting_Shadows : Setting
    {
        public override void ApplyChanges()
        {
            base.ApplyChanges();
            ((UniversalRenderPipelineAsset)GraphicsSettings.defaultRenderPipeline).shadowDistance = value;
        }
    }
}