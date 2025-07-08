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
        private bool init;

        [BoxGroup("Base"), SerializeField, Dropdown("Localization")] private string settingsKey;

        [BoxGroup("Value"), SerializeField] private float defaultValue;
        [BoxGroup("Value"), SerializeField] private float value;
        [BoxGroup("Value"), SerializeField] private bool loop;

        [BoxGroup("ValueRange"), SerializeField] private Vector2 valueRange;

        [BoxGroup("Display"), SerializeField] private DisplayMode displayMode;
        [BoxGroup("Display"), Dropdown("Localization"), SerializeField, ShowIf("displayMode", DisplayMode.KEY)] private string[] keys;

        [BoxGroup("Other Properties"), SerializeField] private SettingsType type;
        [BoxGroup("Other Properties"), SerializeField, ShowIf("type", SettingsType.RANGE)] private float steps;
        [BoxGroup("Other Properties"), SerializeField] private UnityEvent OnValueChange;

        public virtual void Init(string initialValue)
        {
            if (init) return;

            if (!float.TryParse(initialValue, out value))
            {
                SetValue(GetDefault());
            }
            ApplyChanges();

            init = true;
        }

        public void IncrementValue()
        {
            SetValue(GetValue() + 1);
        }

        public void DecrementValue()
        {
            SetValue(GetValue() - 1);
        }

        public void AddValue(float value)
        {
            SetValue(GetValue() + value);
        }

        public virtual void SetValue(float value)
        {
            this.value = value;

            if (loop)
            {
                if (this.value < GetValueRange().x) this.value = GetValueRange().y;
                if (this.value > GetValueRange().y) this.value = GetValueRange().x;
            }
            else
            {
                if (this.value < GetValueRange().x) this.value = GetValueRange().x;
                if (this.value > GetValueRange().y) this.value = GetValueRange().y;
            }
        }

        public virtual void ApplyChanges()
        {
            OnValueChange.Invoke();
        }

        public void SetValueRange(Vector2 newRange)
        {
            valueRange = newRange;
        }

        public UnityEvent GetEvent()
        {
            return OnValueChange;
        }

        public Setting_Local_Json GetLocal()
        {
            return JsonUtility.FromJson<Setting_Local_Json>(Localization_Manager.Instance.GetLocalizedString(settingsKey));
        }

        public virtual Vector2 GetValueRange()
        {
            return valueRange;
        }

        public SettingsType GetSettingsType()
        {
            return type;
        }

        public virtual string GetDisplayValue()
        {
            switch (displayMode)
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

        public virtual float GetDefault()
        {
            return defaultValue;
        }

        public float GetValue()
        {
            return value;
        }

        public string[] GetKeys()
        {
            return keys;
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