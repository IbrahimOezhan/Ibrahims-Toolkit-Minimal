using Sirenix.OdinInspector;
using System;
using IbrahKit;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace IbrahKit
{
    [DefaultExecutionOrder(Execution_Order.input)]
    public class Input_Manager : Manager_Base
    {
        [SerializeField, ReadOnly] private InputType currentInputType;

        public Action<InputType> OnInputChanged;

        public static Input_Manager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;

            }
        }

        private void Update()
        {
            InputType type = currentInputType;

            for (int i = 0; i < InputSystem.devices.Count; i++)
            {
                foreach (InputControl control in InputSystem.devices[i].allControls)
                {
                    switch (control)
                    {
                        case KeyControl key:
                            if (key.wasPressedThisFrame)
                            {
                                currentInputType = InputType.KEYBOARD;
                            }
                            break;
                        case ButtonControl button:
                            if (button.wasPressedThisFrame)
                            {
                                if (IsMouseButton(button))
                                {
                                    currentInputType = InputType.MOUSE;
                                }
                                else currentInputType = InputType.GAMEPAD;
                            }
                            break;
                        case TouchControl touch:
                            if (touch.press.wasPressedThisFrame)
                            {
                                currentInputType = InputType.TOUCHSCREEN;
                            }
                            break;
                    }
                }
            }

            if (currentInputType != type) InputUpdate();
        }

        private bool IsMouseButton(ButtonControl button)
        {
            Mouse mouse = Mouse.current;
            return mouse != null &&
                   (button == mouse.leftButton ||
                    button == mouse.rightButton ||
                    button == mouse.middleButton);
        }

        public InputType GetInputType()
        {
            return currentInputType;
        }

        public void InputUpdate()
        {
            OnInputChanged?.Invoke(currentInputType);
        }
    }
}