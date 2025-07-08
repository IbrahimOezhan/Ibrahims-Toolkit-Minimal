using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace IbrahKit
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
            setting.SetValue(slider.value);
            setting.ApplyChanges();
            UpdateUI();
        }

        public override void UpdateUI()
        {
            base.UpdateUI();
            slider.minValue = setting.GetValueRange().x;
            slider.maxValue = setting.GetValueRange().y;
            //slider.wholeNumbers = setting.GetIsWholeNumber();
            slider.value = setting.GetValue();
        }
    }
}