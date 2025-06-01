using UnityEngine;

namespace TemplateTools
{
    public class Setting_AnisotropicFiltering : Setting
    {
        public override void ApplyChanges()
        {
            base.ApplyChanges();
            switch (value)
            {
                case 0:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                    break;
                case 1:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                    break;
                case 2:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                    break;
            }
        }
    }
}