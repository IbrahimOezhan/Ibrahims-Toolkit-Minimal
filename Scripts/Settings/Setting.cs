using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace IbrahKit
{
    /// <summary>
    /// Provides the base functionality for all elements that the game can have
    /// </summary>
    [Serializable]
    public class Setting
    {
        [BoxGroup("Base"), SerializeField, Dropdown("Localization")] private string settingsKey;

        [BoxGroup("Value"), SerializeField] private float defaultValue;
        [BoxGroup("Value"), SerializeField] protected float value;
        [BoxGroup("Value"), SerializeField] private bool loop;

        [BoxGroup("ValueRange"), SerializeField] protected float minValue;
        [BoxGroup("ValueRange"), SerializeField] protected float maxValue;

        [BoxGroup("Display"), SerializeField] private DisplayMode displayMode;
        [BoxGroup("Display"), Dropdown("Localization"), SerializeField, ShowIf("displayMode", DisplayMode.KEY)] protected string[] keys;

        [BoxGroup("Other Properties"), SerializeField] private SettingsType type;
        [BoxGroup("Other Properties"), SerializeField, ShowIf("type", SettingsType.RANGE)] protected float steps;
        [BoxGroup("Other Properties"), SerializeField] private UnityEvent OnValueChange;

        protected virtual void Init()
        {

        }

        public virtual void LoadSetting(string _value)
        {
            Init();
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

        public UnityEvent GetEvent()
        {
            return OnValueChange;
        }

        public Setting_Local_Json GetLocal()
        {
            return JsonUtility.FromJson<Setting_Local_Json>(Localization_Manager.Instance.GetLocalizedString(settingsKey));
        }

        public virtual Vector2 GetMinMax()
        {
            return new Vector2(minValue, maxValue);
        }

        public SettingsType GetSettingsType()
        {
            return type;
        }

        public virtual string GetDisplayValue()
        {
            switch(displayMode)
            {
                case DisplayMode.RAW:
                    return value.ToString("0.0");
                case DisplayMode.INT:
                    return value.ToString("0");
                case DisplayMode.KEY:
                    return Localization_Manager.Instance.GetLocalizedString(keys[(int)(value / steps)], "");
            }

            return "ERROR";
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

        private enum DisplayMode
        {
            RAW,
            INT,
            KEY,
        }
    }
}