using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TemplateTools
{
    public class Setting_RenderScale : Setting
    {
        public override void ApplyChanges()
        {
            base.ApplyChanges();
            ((UniversalRenderPipelineAsset) GraphicsSettings.defaultRenderPipeline).renderScale = value;
        }
    }
}