namespace TemplateTools
{
    public class Setting_Shadows : Setting
    {
        public override void ApplyChanges()
        {
            base.ApplyChanges();
            Settings_Manager.Instance.pipelineAsset.shadowDistance = value;
        }
    }
}