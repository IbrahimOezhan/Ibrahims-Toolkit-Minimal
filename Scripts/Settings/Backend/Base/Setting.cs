using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace TemplateTools
{
    /// <summary>
    /// Provides the base functionality for all elements that the game can have
    /// </summary>
    public class Setting : MonoBehaviour
    {
        public string settingsKey;

        [FoldoutGroup("Localization"), Dropdown("Localization"), SerializeField] protected string titleKey;
        [FoldoutGroup("Localization")] public bool enableDescription;
        [FoldoutGroup("Localization"), Dropdown("Localization"), SerializeField, ShowIf("enableDescription")] protected string descriptionKey;

        [SerializeField] private float defaultValue;
        public float value;

        [SerializeField, Space(10)] private bool useOtherRangeAsMin;
        [HideIf("useOtherRangeAsMin"), SerializeField] protected float minValue;
        [ShowIf("useOtherRangeAsMin"), SerializeField] private Setting rMinValue;

        [SerializeField, Space(10)] private bool useOtherRangeAsMax;
        [HideIf("useOtherRangeAsMax"), SerializeField] protected float maxValue;
        [ShowIf("useOtherRangeAsMax"), SerializeField] private Setting rMaxValue;

        [Space(10)]
        public bool wholeNumber;
        public bool loop;

        public SettingsType type;

        [ShowIf("type", SettingsType.RANGE)]
        public float steps;

        [SerializeField] private UnityEvent OnValueChange;

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }

        public virtual void LoadSetting(string _value)
        {
            if (!float.TryParse(_value, out value)) value = defaultValue;
        }

        public virtual void ChangeValue(float _value)
        {
            value += _value;
            if (loop)
            {
                if (value < GetMinMax().x) value = GetMinMax().y;
                if (value > GetMinMax().y) value = GetMinMax().x;
            }
            else
            {
                if (value < GetMinMax().x) value = GetMinMax().x;
                if (value > GetMinMax().y) value = GetMinMax().y;
            }
        }

        public virtual void ApplyChanges()
        {
            OnValueChange.Invoke();
        }

        public virtual string GetTitleValue()
        {
            return titleKey;
        }

        public virtual string GetDescriptionValue()
        {
            return descriptionKey;
        }

        public virtual string GetDisplayValue()
        {
            string _value = wholeNumber ? value.ToString("0") : value.ToString("0.0");
            return _value;
        }

        public virtual Vector2 GetMinMax()
        {
            return new Vector2(useOtherRangeAsMin ? rMinValue.value : minValue, useOtherRangeAsMax ? rMaxValue.value : maxValue);
        }
    }
}