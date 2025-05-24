using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TemplateTools
{
    [DefaultExecutionOrder(-1)]
    public class State_Manager : Manager_Base
    {
        [FoldoutGroup("States"), SerializeField, ReadOnly] private int currentState = 0;
        [FoldoutGroup("States"), SerializeField] private List<string> states = new();

        public event Action<string> OnStateChange;

        public static State_Manager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else
            {
                Instance = this;
            }
        }

        private void OnValidate()
        {
            String_Utilities.CreateDropdown(states, "States");
        }

        public void SetCurrentState(string newState)
        {
            SetCurrentState(states.IndexOf(states.Find(x => x == newState)));
        }

        public void SetCurrentState(int index)
        {
            currentState = index;
            StateUpdate();
        }

        public string GetCurrentState()
        {
            return states[currentState];
        }

        public void StateUpdate()
        {
            OnStateChange?.Invoke(states[currentState]);
        }

        public string GetCurrentStateName()
        {
            return states[currentState];
        }
    }
}