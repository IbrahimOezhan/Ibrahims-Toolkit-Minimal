using System;
using System.Collections.Generic;
using TemplateTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class Menu_Pause_Instance : MonoBehaviour
{
    private UI_Input input;
    private string stateBeforePause;

    [SerializeField, Dropdown("States")] private string pausedState;
    [SerializeField] private List<AllowPause> allowPause = new();

    public UI_Menu_Basic Menu;

    public static bool paused;
    public static event Action<bool> OnPause;
    public static Menu_Pause_Instance Instance;

    private void Awake()
    {
        Instance = this;
        input = new();
        input.Enable();
        input.Map.Pause.performed += Pause;
    }

    private void OnDestroy()
    {
        if (input != null)
        {
            input.Map.Pause.performed -= Pause;
            input.Disable();
        }
    }

    public void Pause()
    {
        Pause(new());
    }

    public void Pause(InputAction.CallbackContext _context)
    {
        string currentState = State_Manager.Instance.GetCurrentState();

        AllowPause allow = allowPause.Find(x => x.state == currentState);

        if (allow.allow)
        {
            bool _paused = !paused;

            if (_paused)
            {
                Menu.Enable(null);
                stateBeforePause = currentState;
                State_Manager.Instance.SetCurrentState(pausedState);
                paused = _paused;
            }
            else if (!_paused && Menu.IsEnabled())
            {
                Menu.Disable();
                State_Manager.Instance.SetCurrentState(stateBeforePause);
                paused = _paused;
            }

            Time.timeScale = paused ? 0 : 1;
            OnPause.Invoke(paused);
        }
    }

    private class AllowPause
    {
        public bool allow;
        [Dropdown("States")] public string state;
    }
}