using UnityEngine;

namespace IbrahKit
{
    public class Setting_TextureQuality : Setting
    {
        public override void ApplyChanges()
        {
            base.ApplyChanges();
            QualitySettings.globalTextureMipmapLimit = (int)GetValue();
        }
    }
}
