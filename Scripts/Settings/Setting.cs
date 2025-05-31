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
        [SerializeField, Dropdown("Localization")] private string settingsKey;

        [BoxGroup("Value"), SerializeField] private float defaultValue;
        [BoxGroup("Value"), SerializeField] protected float value;

        [BoxGroup("Value"), SerializeField] private bool wholeNumber;
        [BoxGroup("Value"), SerializeField] private bool loop;

        [BoxGroup("Min"), SerializeField] private bool useOtherRangeAsMin;
        [BoxGroup("Min"), HideIf("useOtherRangeAsMin"), SerializeField] protected float minValue;
        [BoxGroup("Min"), ShowIf("useOtherRangeAsMin"), SerializeField] private Setting rMinValue;

        [BoxGroup("Max"),SerializeField] private bool useOtherRangeAsMax;
        [BoxGroup("Max"), HideIf("useOtherRangeAsMax"), SerializeField] protected float maxValue;
        [BoxGroup("Max"), ShowIf("useOtherRangeAsMax"), SerializeField] private Setting rMaxValue;


        [BoxGroup("Other Properties"), SerializeField] private SettingsType type;
        [BoxGroup("Other Properties"), ShowIf("type", SettingsType.RANGE), SerializeField] private float steps;

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

        public void LoadDefault()
        {
            ChangeValue(defaultValue - value);
            ApplyChanges();
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

        public void SetValue(float value)
        {
            this.value = value;
        }

        public SettingsType GetSettingsType()
        {
            return type;
        }

        public UnityEvent GetEvent()
        {
            return OnValueChange;
        }

        public virtual Vector2 GetMinMax()
        {
            return new Vector2(useOtherRangeAsMin ? rMinValue.value : minValue, useOtherRangeAsMax ? rMaxValue.value : maxValue);
        }

        public Setting_Local_Json GetLocal()
        {
            return JsonUtility.FromJson< Setting_Local_Json >( Localization_Manager.Instance.GetLocalizedString(settingsKey));
        }

        public virtual string GetDisplayValue()
        {
            string _value = wholeNumber ? value.ToString("0") : value.ToString("0.0");
            return _value;
        }

        public string GetKey()
        {
            return settingsKey;
        }

        public float GetStep()
        {
            return steps;
        }

        public float GetValue()
        {
            return value;
        }

        public bool GetLoop()
        {
            return loop;
        }

        public bool GetIsWholeNumber()
        {
            return wholeNumber;
        }
    }
}