using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IbrahKit
{
    [DefaultExecutionOrder(Execution_Order.ui)]
    public class UI_Manager : Manager_Base
    {
        public const string UILAYOUTKEY = "UILayouts";

        private bool hidden;

        [SerializeField] private UI_Fitter_Config_SO defaultConfig;
        [SerializeField] private UI_Menu_Config_SO defaultMenuConfig;
        [SerializeField] private UI_Styling_Config_SO defaultUIStyle;
        [SerializeField] private KeyMap keyMap;

        [SerializeField, Dropdown(UILAYOUTKEY)] private List<string> activeLayouts;

        [SerializeField] private List<UI_Menu_Basic> activeMenus = new();

        public Action<bool> OnHide;
        public Action OnHover;
        public Action OnClick;
        public Action<UI_Menu_Basic, StateMode> OnCustomTR;

        public static UI_Manager Instance;

        public static bool Exists(out UI_Manager manager, bool throwWarningIfDoesnt = true)
        {
            manager = Instance;

            bool exists = Instance != null && Instance.gameObject != null;

            if (!exists && throwWarningIfDoesnt) Debug.LogWarning($"{nameof(UI_Manager)} doesnt exist");

            return exists;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else
            {
                Instance = this;
            }
        }

        private void Update()
        {
            if (Keyboard.current[keyMap.screenshot].wasPressedThisFrame)
            {
                Screenshot();
            }
            if (Keyboard.current[keyMap.screenshotNoUI].wasPressedThisFrame)
            {
                ScreenshotNoUI();
            }
            if (Keyboard.current[keyMap.hideUI].wasPressedThisFrame)
            {
                Hide();
            }
        }

        private void OnDisable()
        {
            if (Instance != this) return;
        }

        private void Hide()
        {
            hidden = !hidden;
            UpdateHide();
        }

        public void UpdateHide()
        {
            OnHide?.Invoke(hidden);
        }

        public bool ShowLayout(List<string> layouts)
        {
            return activeLayouts.Intersect(layouts).Count() > 0;
        }

        public void Screenshot()
        {
            Basic_Utilities.Screenshot();
        }

        public void ScreenshotNoUI()
        {
            ScreenshotNoResult();
        }

        private async void ScreenshotNoResult()
        {
            Hide();

            await Task.Yield();

            Basic_Utilities.Screenshot();

            await Task.Yield();

            Hide();
        }

        public void OnUIHover()
        {
            OnHover?.Invoke();
        }

        public void OnUIClick()
        {
            OnClick?.Invoke();
        }

        public void Transition(UI_Menu_Basic menuIn, UI_Menu_Basic menuOut, FadeMode fadeMode, float _fadeTime)
        {
            StartCoroutine(TransitionRoutine(menuIn, menuOut, fadeMode, _fadeTime));
        }

        public IEnumerator TransitionRoutine(UI_Menu_Basic menuIn, UI_Menu_Basic menuOut, FadeMode fadeMode, float _fadeTime)
        {
            yield return StartCoroutine(FadeRoutine(menuIn, StateMode.Disable, fadeMode, _fadeTime));
            yield return StartCoroutine(FadeRoutine(menuOut, StateMode.Enable, fadeMode, _fadeTime));
            menuOut.SetPreviousMenu(menuIn);
        }

        public void Fade(UI_Menu_Basic menu, StateMode stateMode, FadeMode fadeMode, float _fadeTime)
        {
            StartCoroutine(FadeRoutine(menu, stateMode, fadeMode, _fadeTime));
        }

        public IEnumerator FadeRoutine(UI_Menu_Basic menu, StateMode stateMode, FadeMode fadeMode, float _fadeTime)
        {
            switch (stateMode)
            {
                case StateMode.Enable:

                    menu.SetActive(true);

                    switch (fadeMode)
                    {
                        case FadeMode.None:
                            menu.SetAlpha(1);
                            menu.SetInteractable(true);
                            break;
                        case FadeMode.Time:
                            while (menu.GetAlpha() < 1)
                            {
                                menu.SetAlpha(menu.GetAlpha() + Time.deltaTime / _fadeTime);
                                yield return null;
                            }
                            menu.SetInteractable(true);
                            break;
                        case FadeMode.Custom:
                            OnCustomTR?.Invoke(menu, stateMode);
                            break;
                    }

                    AddMenu(menu);

                    break;
                case StateMode.Disable:

                    switch (fadeMode)
                    {
                        case FadeMode.None:
                            menu.SetAlpha(0);
                            menu.SetInteractable(false);
                            menu.SetActive(false);
                            break;
                        case FadeMode.Time:
                            menu.SetInteractable(false);
                            while (menu.GetAlpha() > 0)
                            {
                                menu.SetAlpha(menu.GetAlpha() - Time.deltaTime / _fadeTime);
                                yield return null;
                            }
                            menu.SetActive(false);
                            break;
                        case FadeMode.Custom:
                            OnCustomTR?.Invoke(menu, stateMode);
                            break;
                    }

                    RemoveMenu(menu);

                    break;
            }
        }

        public void AddMenu(UI_Menu_Basic menu)
        {
            activeMenus.Add(menu);
        }

        public void RemoveMenu(UI_Menu_Basic menu)
        {
            activeMenus.Remove(menu);
        }

        public UI_Styling_Config_SO GetDefaultStyle()
        {
            return defaultUIStyle;
        }

        public UI_Menu_Config_SO GetDefaultMenuConfig()
        {
            return defaultMenuConfig;
        }

        public UI_Fitter_Config_SO GetDefaultUIConfig()
        {
            return defaultConfig;
        }
    }

    public enum FadeMode
    {
        None,
        Time,
        Custom,
    }

    public enum StateMode
    {
        Enable,
        Disable,
    }

    public enum InputType
    {
        KEYBOARD,
        MOUSE,
        GAMEPAD,
        TOUCHSCREEN,
    }
}