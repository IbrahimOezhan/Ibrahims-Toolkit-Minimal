using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace TemplateTools
{
    public class UI_Menu_Basic : MonoBehaviour
    {
        protected InputType lastInputType;
        protected UI_Menu_Basic previousMenu;
        protected List<UI_Base> menuUI;

        [FoldoutGroup("MenuSettings", order: -2), SerializeField] protected CanvasGroup enabledGroup;
        [FoldoutGroup("MenuSettings", order: -1), SerializeField] protected CanvasGroup hiddenGroup;

        [FoldoutGroup("MenuSettings", order: -1), SerializeField] protected Button defaultButton;
        [FoldoutGroup("MenuSettings", order: -1), SerializeField] protected bool hideOnPause;
        [FoldoutGroup("MenuSettings", order: -1), SerializeField] protected bool preventHideByButton;
        [FoldoutGroup("MenuSettings", order: -1), SerializeField] protected bool disableOnStart;

        [FoldoutGroup("Transition", order: 0), SerializeField] protected UI_Menu_Basic overrideBackMenu;
        [FoldoutGroup("Transition", order: 0), SerializeField] private List<MenuTransition> transitions;

        public static Action<MenuTransition, UI_Menu_Basic> OnMenuTransition;

        protected virtual void Awake()
        {
            InitMenuContent();
        }

        protected virtual void Start()
        {
            if (IsEnabled()) UI_Manager.Instance.AddMenu(this);
            if (disableOnStart) Disable();
        }

        protected virtual void OnEnable()
        {
            if (hideOnPause) Menu_Pause_Instance.OnPause += OnPause;

            if (UI_Manager.Instance != null)
            {
                UI_Manager.Instance.OnHide += Hide;
                Hide(UI_Manager.Instance.hidden);
            }
        }

        protected virtual void OnDisable()
        {
            if (hideOnPause)
            {
                Menu_Pause_Instance.OnPause -= OnPause;
            }

            if (UI_Manager.Instance)
            {
                UI_Manager.Instance.OnHide -= Hide;
            }
        }

        protected virtual void OnDestroy()
        {

        }

        private void OnRectTransformDimensionsChange()
        {
            MenuUpdate();
        }

        private void OnApplicationFocus(bool _focus)
        {
            MenuUpdate();
        }

        public bool IsEnabled()
        {
            return enabledGroup.alpha == 1;
        }

        public bool IsDisabled()
        {
            return enabledGroup.alpha == 0;
        }

        public void SetAlpha(float alpha)
        {
            enabledGroup.alpha = alpha;
        }

        public float GetAlpha()
        {
            return enabledGroup.alpha;
        }

        public void SetInteractable(bool val)
        {
            enabledGroup.interactable = val;
        }

        public void SetActive(bool val)
        {
            gameObject.SetActive(val);

            if(val)
            {
                OnMenuEnabled();
            }
            else OnMenuDisable();
        }

        protected void InitMenuContent()
        {
            menuUI = Transform_Utilities.GetChildren<UI_Base>(transform);
            foreach(UI_Base child in  menuUI)
            {
                child.SetParentMenu(this);
            }

            MenuUpdate();
        }

        public void SetPreviousMenu(UI_Menu_Basic menu)
        {
            previousMenu = menu;
        }

        protected void MenuUpdate()
        {
            foreach (UI_Base _ui in menuUI)
            {
                if (_ui != null && _ui.TryGetComponent<IMenuUpdate>(out var _element)) _element.MenuUpdate();
            }
        }

        [BoxGroup("Buttons", order: -3), Button]
        public void Enable()
        {
            Enable(null);
        }

        [BoxGroup("Buttons", order: -3), Button]
        public void Disable()
        {
            Disable(FadeMode.None, 0);
        }

        public void Enable(UI_Menu_Basic _enabledFrom, FadeMode fadeMode = FadeMode.None, float _fadeTime = 0)
        {
            if (UI_Manager.Instance != null)
            {
                SetPreviousMenu(_enabledFrom);
                UI_Manager.Instance.Fade(this, StateMode.Enable, fadeMode, _fadeTime);
            }
            else
            {
                SetActive(true);
                enabledGroup.alpha = 1;
                enabledGroup.interactable = true;
            }
        }

        public void Disable(FadeMode fadeMode = FadeMode.None, float _fadeTime = 0)
        {
            if (UI_Manager.Instance != null)
            {
                UI_Manager.Instance.Fade(this, StateMode.Disable, fadeMode, _fadeTime);
            }
            else
            {
                SetActive(false);
                enabledGroup.alpha = 0;
                enabledGroup.interactable = false;
            }
        }

        protected virtual void OnMenuEnabled()
        {

        }

        protected virtual void OnMenuDisable()
        {

        }

        public void MenuTransition(UI_Menu_Basic _menu)
        {
            MenuTransition(_menu, null);
        }

        public void MenuTransition(UI_Menu_Basic _menu, UI_Menu_Basic _overrideBackMenu = null)
        {
            if (_overrideBackMenu != null) _menu.overrideBackMenu = _overrideBackMenu;
            _menu.Enable(this);
            Disable();
        }

        public void MenuTransition(int _index)
        {
            MenuTransition transition = transitions[_index];
            UI_Manager.Instance.Transition(this, transition.menu, transition.mode, transition.fadeTime);
            OnMenuTransition?.Invoke(transition, this);
        }

        public void MenuTransitionToPrevious()
        {
            MenuTransition(previousMenu);
        }

        private void OnPause(bool state)
        {
            Hide(state);
        }

        protected void Hide(bool hide)
        {
            if (preventHideByButton) return;
            if (hide) hiddenGroup.alpha = 0;
            else hiddenGroup.alpha = 1;
        }

        public void SetParams(CanvasGroup enabled, CanvasGroup hidden)
        {
            enabledGroup = enabled;
            hiddenGroup = hidden;
        }
    }

    [System.Serializable]
    public class MenuTransition
    {
        public UI_Menu_Basic menu;
        public float fadeTime;
        public FadeMode mode;
    }
}