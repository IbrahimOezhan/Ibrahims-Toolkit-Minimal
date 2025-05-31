using UnityEngine;

namespace TemplateTools
{
    public class Setting_TextureQuality : Setting_KeyValue
    {
        public override void ApplyChanges()
        {
            base.ApplyChanges();
            QualitySettings.globalTextureMipmapLimit = (int)value;
        }
    }
}
