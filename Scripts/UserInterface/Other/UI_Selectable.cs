using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TemplateTools
{
    public class UI_Selectable : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ICursorHandler
    {
        [SerializeField] private SelectedState selectedState;

        [SerializeField] private AnimationState animState;

        [ShowIf("animState", AnimationState.Colors), SerializeField] private ColorTransition colorTransition = new();
        [ShowIf("animState", AnimationState.Animation), SerializeField] private AnimationTransition animationTransition = new();
        [ShowIf("animState", AnimationState.Scale), SerializeField] private ScaleTransition scaleTransition = new();

        [SerializeField, FoldoutGroup("Navigation")] private UI_Selectable up;
        [SerializeField, FoldoutGroup("Navigation")] private UI_Selectable down;
        [SerializeField, FoldoutGroup("Navigation")] private UI_Selectable left;
        [SerializeField, FoldoutGroup("Navigation")] private UI_Selectable right;
        [SerializeField, FoldoutGroup("Navigation")] private RectTransform rect;

        [SerializeField] public UnityEvent OnClickEvent;
        [SerializeField] public Action OnClickAction;
        [SerializeField] public bool interactable;

        public float alignmentTolerance = 0.1f;

        public static UI_Selectable currentlySelected;

        protected override void Awake()
        {
            if (rect == null) rect = GetComponent<RectTransform>();
        }

        protected override void OnDisable()
        {
            selectedState = SelectedState.None;
            Visualize();
            DeSelect();
        }

        public void SetupNavigation(List<UI_Selectable> list)
        {
            float minLeftDist = Mathf.Infinity;
            float minRightDist = Mathf.Infinity;
            float minUpDist = Mathf.Infinity;
            float minDownDist = Mathf.Infinity;

            float minDiagLeftDist = Mathf.Infinity;
            float minDiagRightDist = Mathf.Infinity;
            float minDiagUpDist = Mathf.Infinity;
            float minDiagDownDist = Mathf.Infinity;

            Vector2 currentPos = transform.position;

            foreach (var point in list)
            {
                if (point.transform == transform) continue;

                Vector2 direction = (Vector2)point.transform.position - currentPos;
                float dist = Vector2.Distance(currentPos, point.transform.position);

                // === Left Navigation ===
                if (direction.x < 0)
                {
                    float distX = Mathf.Abs(direction.x);

                    if (Mathf.Abs(direction.y) <= alignmentTolerance)
                    {
                        if (distX < minLeftDist)
                        {
                            minLeftDist = distX;
                            left = point;
                        }
                    }
                    else if (Mathf.Abs(direction.y) < Mathf.Abs(direction.x) && dist < minDiagLeftDist)
                    {
                        minDiagLeftDist = dist;
                        left = left == null ? point : left;
                    }
                }

                // === Right Navigation ===
                if (direction.x > 0)
                {
                    float distX = Mathf.Abs(direction.x);

                    if (Mathf.Abs(direction.y) <= alignmentTolerance)
                    {
                        if (distX < minRightDist)
                        {
                            minRightDist = distX;
                            right = point;
                        }
                    }
                    else if (Mathf.Abs(direction.y) < Mathf.Abs(direction.x) && dist < minDiagRightDist)
                    {
                        minDiagRightDist = dist;
                        right = right == null ? point : right;
                    }
                }

                // === Up Navigation ===
                if (direction.y > 0)
                {
                    float distY = Mathf.Abs(direction.y);

                    if (Mathf.Abs(direction.x) <= alignmentTolerance)
                    {
                        if (distY < minUpDist)
                        {
                            minUpDist = distY;
                            up = point;
                        }
                    }
                    else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y) && dist < minDiagUpDist)
                    {
                        minDiagUpDist = dist;
                        up = up == null ? point : up;
                    }
                }

                // === Down Navigation ===
                if (direction.y < 0)
                {
                    float distY = Mathf.Abs(direction.y);

                    if (Mathf.Abs(direction.x) <= alignmentTolerance)
                    {
                        if (distY < minDownDist)
                        {
                            minDownDist = distY;
                            down = point;
                        }
                    }
                    else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y) && dist < minDiagDownDist)
                    {
                        minDiagDownDist = dist;
                        down = down == null ? point : down;
                    }
                }
            }
        }

        public void Navigation(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    if (direction.x < 0)
                    {
                        Navigate(left);
                    }
                    else
                    {
                        Navigate(right);
                    }
                }
                else
                {
                    if (direction.y < 0)
                    {
                        Navigate(down);
                    }
                    else
                    {
                        Navigate(up);
                    }
                }
            }
        }

        public void Navigate(UI_Selectable selectable)
        {
            if (selectable == null) return;
            selectable.Select();
            DeSelect();
        }

        public virtual void Select()
        {
            selectedState = SelectedState.Hovering;
            Visualize();
            currentlySelected = this;
        }

        public virtual void DeSelect()
        {
            selectedState = SelectedState.None;
            Visualize();
            if (currentlySelected == this) currentlySelected = null;
        }

        public void Visualize()
        {
            switch (animState)
            {
                case AnimationState.Colors:
                    colorTransition.Apply(selectedState, gameObject);
                    break;
                case AnimationState.Animation:
                    animationTransition.Apply(selectedState, gameObject);
                    break;
                case AnimationState.Scale:
                    scaleTransition.Apply(selectedState, gameObject);
                    break;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Hover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            selectedState = SelectedState.None;
            Visualize();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Press();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            selectedState = SelectedState.None;
            Visualize();
        }

        public void Press()
        {
            selectedState = SelectedState.Pressed;
            Visualize();
            OnClickEvent.Invoke();
        }

        public void Hover()
        {
            selectedState = SelectedState.Hovering;
            Visualize();
        }

        [Serializable]
        protected class AnimationTransition : SelectableTransition
        {
            [SerializeField] private Animator animator;
            [SerializeField] private string none = "None";
            [SerializeField] private string hovering = "Hovering";
            [SerializeField] private string pressed = "Pressed";

            public override void Apply(SelectedState state, GameObject go)
            {
                base.Apply(state, go);

                switch (state)
                {
                    case SelectedState.Hovering:
                        animator.Play(hovering);
                        break;
                    case SelectedState.None:
                        animator.Play(none);
                        break;
                    case SelectedState.Pressed:
                        animator.Play(pressed);
                        break;
                }
            }
        }

        [Serializable]
        protected class ColorTransition : SelectableTransition
        {
            [SerializeField] private Graphic graphic;
            [SerializeField] private Color none;
            [SerializeField] private Color hovering;
            [SerializeField] private Color pressed;

            public override void Apply(SelectedState state, GameObject go)
            {
                base.Apply(state, go);

                Color c = new();

                switch (state)
                {
                    case SelectedState.Hovering:
                        c = hovering;
                        break;
                    case SelectedState.Pressed:
                        c = pressed;
                        break;
                    case SelectedState.None:
                        c = none;
                        break;
                }

                graphic.color = c;
            }
        }

        [Serializable]
        protected class ScaleTransition : SelectableTransition
        {
            [SerializeField] private RectTransform rect;
            [SerializeField] private float none;
            [SerializeField] private float hovering;
            [SerializeField] private float pressed;

            public override void Apply(SelectedState state, GameObject go)
            {
                base.Apply(state, go);

                if (rect == null) rect = go.GetComponent<RectTransform>();

                Vector3 scale = new();

                switch (state)
                {
                    case SelectedState.Hovering:
                        scale = new(hovering, hovering);
                        break;
                    case SelectedState.Pressed:
                        scale = new(pressed, pressed);
                        break;
                    case SelectedState.None:
                        scale = new(none, none);
                        break;
                }

                rect.localScale = scale;
            }
        }

        protected class SelectableTransition
        {
            public virtual void Apply(SelectedState state, GameObject go)
            {

            }
        }

        protected enum SelectedState
        {
            None,
            Hovering,
            Pressed,
        }

        protected enum AnimationState
        {
            None,
            Colors,
            Animation,
            Scale,
        }
    }
}