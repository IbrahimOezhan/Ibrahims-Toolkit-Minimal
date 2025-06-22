using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace IbrahKit
{
    public class UI_Selectable : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ICursorHandler
    {
        [TabGroup("Transition Settings"), SerializeField]
        private SelectedState selectedState;

        [TabGroup("Transition Settings"), SerializeReference]
        private List<SelectableTransition> transitions = new();

        [TabGroup("Transition Settings"), SerializeReference]
        private List<SelectableTransition> transitionsInteractable = new();

        [TabGroup("Transition Settings"), SerializeReference]
        private List<SelectableTransition> transitionsNotInteractable = new();

        [TabGroup("Navigation Settings"), SerializeField]
        private UI_Selectable up;

        [TabGroup("Navigation Settings"), SerializeField]
        private UI_Selectable down;

        [TabGroup("Navigation Settings"), SerializeField]
        private UI_Selectable left;

        [TabGroup("Navigation Settings"), SerializeField]
        private UI_Selectable right;

        [TabGroup("Navigation Settings"), SerializeField]
        private RectTransform rect;

        [TabGroup("Navigation Settings"), SerializeField]
        private float alignmentTolerance = 0.1f;

        [TabGroup("Navigation Settings"), SerializeField]
        public bool interactable = true;

        [TabGroup("Events"), SerializeField]
        public UnityEvent OnClickEvent;

        [NonSerialized]
        public Action OnClickAction;

        [NonSerialized]
        public static UI_Selectable currentlySelected;

        protected override void Awake()
        {
            if (rect == null) rect = GetComponent<RectTransform>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if(UI_Navigation_Manager.Instance != null)
            {
                UI_Navigation_Manager.Instance.AddSelectable(this);
                UI_Navigation_Manager.Instance.UpdateSelectables();
            }

            Visualize();
        }

        protected override void OnDisable()
        {
            selectedState = SelectedState.None;

            Visualize();
            DeSelect();

            if (UI_Navigation_Manager.Instance != null)
            {
                UI_Navigation_Manager.Instance.RemoveSelectable(this);
                UI_Navigation_Manager.Instance.UpdateSelectables();
            }
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
            for (int i = 0; i < transitions.Count; i++)
            {
                transitions[i].Apply(selectedState, gameObject);
            }

            if (interactable)
            {
                for (int i = 0; i < transitionsInteractable.Count; i++)
                {
                    transitionsInteractable[i].Apply(selectedState, gameObject);
                }
            }
            else
            {
                for (int i = 0; i < transitionsNotInteractable.Count; i++)
                {
                    transitionsNotInteractable[i].Apply(selectedState, gameObject);
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Hover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Exit();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (interactable)
            {
                Press();
            }
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

            if (interactable) UI_Manager.Instance.OnUIClick();
        }

        public void Hover()
        {
            selectedState = SelectedState.Hovering;

            if(interactable) UI_Manager.Instance.OnUIHover();

            Visualize();
        }

        public void Exit()
        {
            selectedState = SelectedState.None;

            Visualize();
        }

        public void SetInteractable(bool value)
        {
            interactable = value;

            Visualize();
        }

        public bool GetInteractable()
        {
            return interactable;
        }
    }
}