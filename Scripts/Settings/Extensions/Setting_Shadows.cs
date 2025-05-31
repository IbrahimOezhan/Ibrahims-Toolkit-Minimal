using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TemplateTools
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