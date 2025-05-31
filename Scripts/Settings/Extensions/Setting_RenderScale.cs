namespace TemplateTools
{
    public class Setting_RenderScale : Setting
    {
        public override void ApplyChanges()
        {
            base.ApplyChanges();
            Settings_Manager.Instance.pipelineAsset.renderScale = value;
        }
    }
}