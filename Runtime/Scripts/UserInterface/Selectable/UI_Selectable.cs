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
        [BoxGroup("Transition"), SerializeField] private SelectedState selectedState;
        [BoxGroup("Transition"), SerializeReference] private List<SelectableTransition> transitions = new();

        [BoxGroup("Navigation"), SerializeField] private UI_Selectable up;
        [BoxGroup("Navigation"), SerializeField] private UI_Selectable down;
        [BoxGroup("Navigation"), SerializeField] private UI_Selectable left;
        [BoxGroup("Navigation"), SerializeField] private UI_Selectable right;
        [BoxGroup("Navigation"), SerializeField] private RectTransform rect;
        [BoxGroup("Navigation"), SerializeField] private float alignmentTolerance = 0.1f;
        [BoxGroup("Navigation"), SerializeField] public bool interactable;

        [SerializeField] public UnityEvent OnClickEvent;

        public Action OnClickAction;

        public static UI_Selectable currentlySelected;

        protected override void Awake()
        {
            if (rect == null) rect = GetComponent<RectTransform>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            UI_Navigation_Manager.Instance.AddSelectable(this);
            UI_Navigation_Manager.Instance.UpdateSelectables();
        }

        protected override void OnDisable()
        {
            selectedState = SelectedState.None;
            Visualize();
            DeSelect();

            UI_Navigation_Manager.Instance.RemoveSelectable(this);
            UI_Navigation_Manager.Instance.UpdateSelectables();
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
    }
}
