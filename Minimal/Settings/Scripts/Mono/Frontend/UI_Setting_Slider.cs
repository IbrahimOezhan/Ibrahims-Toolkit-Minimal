using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace TemplateTools
{
    public class UI_Setting_Slider : UI_Setting
    {
        [FoldoutGroup("UI"), SerializeField] private Slider slider;

        private void Start()
        {
            slider.onValueChanged.AddListener(ChangeValue);
        }

        public override void ChangeValue(float _value)
        {
            if (setting == null) return;
            setting.value = slider.value;
            setting.ApplyChanges();
            UpdateUI();
        }

        public override void UpdateUI()
        {
            base.UpdateUI();
            slider.minValue = setting.GetMinMax().x;
            slider.maxValue = setting.GetMinMax().y;
            slider.wholeNumbers = setting.wholeNumber;
            slider.value = setting.value;
        }
    }

    [System.Serializable]
    public class SliderData
    {
        public UI_Localization title;
        public UI_Localization value;
        public Slider slider;
    }
}