using System.Collections.Generic;
using Sirenix.OdinInspector;
using TemplateTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Navigation_Manager : MonoBehaviour
{
    public static UI_Navigation_Manager Instance;

    private InputTypeNavigation currentType;
    private UI_Input input;

    [SerializeField] private List<UI_Selectable> activeSelectables = new();
    [SerializeField] private List<InputType> supportedUINavigationMethods = new();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Input_Manager.Instance.OnInputChanged += OnInputChanged;
            Input_Manager.Instance.InputUpdate();

            input = new();
            input.Enable();

            input.Navigation.Keyboard.performed += OnKeyboardInput;
            input.Navigation.Gamepad.performed += OnGamepadInput;
            input.Navigation.ConfirmKeyboard.performed += ComfirmKeyboard;
            input.Navigation.ConfirmKeyboard.canceled += LetGoKeyboard;
        }
    }

    private void OnDestroy()
    {
        if(input  != null)
        {
            input.Navigation.Keyboard.performed -= OnKeyboardInput;
            input.Navigation.Gamepad.performed -= OnGamepadInput;
            input.Navigation.ConfirmKeyboard.performed -= ComfirmKeyboard;
            input.Navigation.ConfirmKeyboard.canceled -= LetGoKeyboard;
            input.Disable();
            input.Dispose();
        }

        Input_Manager.Instance.OnInputChanged -= OnInputChanged;
    }

    public void AddSelectable(UI_Selectable selectable)
    {
        activeSelectables.Add(selectable);
    }

    public void RemoveSelectable(UI_Selectable selectable)
    {
        activeSelectables.Remove(selectable);
    }

    [Button]
    public void UpdateSelectables()
    {
        activeSelectables.RemoveAll(x => x == null);

        foreach(UI_Selectable selectable in activeSelectables)
        {
            selectable.SetupNavigation(activeSelectables);
        }
    }


    public UI_Selectable test1;
    public UI_Selectable test2;
    [Button]
    public void Check()
    {
        Debug.Log(test2.transform.position - test1.transform.position);
    }

    private void OnKeyboardInput(InputAction.CallbackContext context)
    {
        Vector2 moveDir = context.ReadValue<Vector2>();
        Navigate(moveDir);
    }

    private void ComfirmKeyboard(InputAction.CallbackContext context)
    {
        UI_Selectable.currentlySelected?.Press();
    }

    private void LetGoKeyboard(InputAction.CallbackContext context)
    {
        UI_Selectable.currentlySelected?.Hover();
    }

    private void OnGamepadInput(InputAction.CallbackContext context)
    {
        Vector2 moveDir = context.ReadValue<Vector2>();
        Navigate(moveDir);
    }

    public void Navigate(Vector2 dir)
    {
        UI_Selectable.currentlySelected?.Navigation(dir);
    }

    private void OnInputChanged(InputType type)
    {
        if(!IsSupported(type))
        {
            return;
        }

        InputTypeNavigation newType = GetInputType(type);

        if (currentType != newType)
        {
            switch(newType)
            {
                case InputTypeNavigation.BUTTONS:
                    if(activeSelectables.Count > 0) activeSelectables[0].Select();
                    break;
                case InputTypeNavigation.POINT:
                    UI_Selectable.currentlySelected?.DeSelect();
                    break;
            }
        }

        currentType = newType;
    }

    public bool IsSupported(InputType type)
    {
        return supportedUINavigationMethods.Contains(type);
    }

    private InputTypeNavigation GetInputType(InputType type)
    {
        switch(type)
        {
            case InputType.GAMEPAD:
                return InputTypeNavigation.BUTTONS;
            case InputType.KEYBOARD:
                return InputTypeNavigation.BUTTONS;
            case InputType.MOUSE:
                return InputTypeNavigation.POINT;
            case InputType.TOUCHSCREEN:
                return InputTypeNavigation.POINT;
        }

        return InputTypeNavigation.POINT;
    }

    private enum InputTypeNavigation
    {
        POINT,
        BUTTONS,
    }
}
