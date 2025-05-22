using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TemplateTools
{
    [DefaultExecutionOrder(-1)]
    public class UI_Manager : Manager_Base
    {
        public int[] uiLayouts;
        public UI_Config_So defaultConfig;
        public UI_Menu_Config defaultMenuConfig;
        public UI_Style_SO defaultStyle;

        public List<UI_Menu_Basic> activeMenus = new();

        [ReadOnly] public bool hidden;

        public Action<bool> OnHide;
        public Action OnHover;
        public Action OnClick;
        public Action OnLayoutChanged;
        public Action<UI_Menu_Basic, StateMode> OnCustomTR;
        public Key screenshotKey;
        public Key screenshotNoUIKey;
        public Key hideKey; 

        public static UI_Manager Instance;

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
            if (Keyboard.current[screenshotKey].wasPressedThisFrame)
            {
                Screenshot();
            }
            if (Keyboard.current[screenshotNoUIKey].wasPressedThisFrame)
            {
                ScreenshotNoUI();
            }
            if (Keyboard.current[hideKey].wasPressedThisFrame)
            {
                Hide();
            }
        }

        private void OnDisable()
        {
            if (Instance != this) return;
        }

        private void Hide(InputAction.CallbackContext _callbackContext)
        {
            Hide();
        }

        private void Hide()
        {
            hidden = !hidden;
            OnHide?.Invoke(hidden);
        }

        public bool ShowLayout(int layout)
        {
            bool show = false;

            for (int i = 0; i < uiLayouts.Length; i++)
            {
                if (uiLayouts[i] == layout) show = true;
            }

            return show;
        }

        public void Screenshot()
        {
            Template_Utilities.Screenshot();
        }

        public void ScreenshotNoUI()
        {
            ScreenshotNoResult();
        }

        private async void ScreenshotNoResult()
        {
            Hide();

            await Task.Yield();

            Template_Utilities.Screenshot();

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

        public void ChangeLayout(int[] newLayouts)
        {
            uiLayouts = newLayouts;
            OnLayoutChanged?.Invoke();
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