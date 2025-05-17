using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TemplateTools
{
    public class Cursor_Manager : MonoBehaviour
    {
        private string currentState;
        private bool found;
        private bool isVisible;
        private CursorInput input;
        private Camera mainCamera;

        private CursorState cursorState;
        private CursorState preCursorState;
        private InputType inputType;

        [SerializeField] private bool enableCustomCursor;
        [ShowIf("enableCustomCursor"), SerializeField] private Sprite noneCursor;
        [ShowIf("enableCustomCursor"), SerializeField] private Sprite hoveringCursor;
        [ShowIf("enableCustomCursor"), SerializeField] private Sprite downCursor;

        [SerializeField] private List<CursorVisibilty> cursorVisibility;
        [SerializeField] private Image customCursor;
        [SerializeField] private RectTransform canvas;

        public static Cursor_Manager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                input = new();
                input.Enable();
            }
        }

        private void Start()
        {
            State_Manager.Instance.OnStateChange += OnStateChanged;
            Input_Manager.Instance.OnInputChanged += OnInputTypeChanged;
            State_Manager.Instance.StateUpdate();
            Input_Manager.Instance.InputUpdate();

            UpdateCursor();
        }

        private void Update()
        {
            switch (inputType)
            {
                case InputType.KEYBOARD:
                case InputType.MOUSE:

                    isVisible = IsVisible(currentState);

                    if (enableCustomCursor)
                    {
                        OnCustomCursor();
                    }
                    else
                    {
                        if (isVisible)
                        {
                            Cursor.visible = true;
                            Cursor.lockState = CursorLockMode.Confined;
                        }
                        else
                        {
                            Cursor.visible = false;
                            Cursor.lockState = CursorLockMode.Locked;
                        }

                        customCursor.gameObject.SetActive(true);
                    }

                    break;
                case InputType.GAMEPAD:

                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;

                    break;
            }
        }

        private void OnDestroy()
        {
            if (State_Manager.Instance) State_Manager.Instance.OnStateChange -= OnStateChanged;
            if (Input_Manager.Instance) Input_Manager.Instance.OnInputChanged -= OnInputTypeChanged;
            if (input != null)
            {
                input.Disable();
                input.Dispose();
            }
        }

        private void OnCustomCursor()
        {
            preCursorState = cursorState;

            Vector2 mousePos = input.Map.MousePos.ReadValue<Vector2>();

            found = false;

            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log("UI Hit");

                PointerEventData pointerData = new(EventSystem.current)
                {
                    position = mousePos
                };

                List<RaycastResult> results = new();
                EventSystem.current.RaycastAll(pointerData, results);

                if (results.Count > 0 && results[0].gameObject.GetComponent<ICursorHandler>() != null)
                {
                    found = true;
                }
            }
            else
            {
                if (mainCamera == null) mainCamera = Camera.main;

                Vector2 mousePosWorld = mainCamera.ScreenToWorldPoint(mousePos);

                RaycastHit2D hit2D = Physics2D.Raycast(mousePosWorld, Vector2.zero);

                if (hit2D.transform != null)
                {
                    //Debug.Log("Hit: " + hit2D.transform.gameObject.name);

                    if (hit2D.transform.gameObject.GetComponent<ICursorHandler>() != null)
                    {
                        found = true;
                    }
                }
            }

            if (found)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame || (cursorState == CursorState.Down && Mouse.current.leftButton.isPressed))
                {
                    SetCursor(CursorState.Down);
                }
                else
                {
                    SetCursor(CursorState.Hovering);
                }
            }
            else
            {
                SetCursor(CursorState.None);
            }

            // Get camera rect in screen pixels:
            float camX = mainCamera.rect.x * Screen.width;
            float camY = mainCamera.rect.y * Screen.height;
            float camWidth = mainCamera.rect.width * Screen.width;
            float camHeight = mainCamera.rect.height * Screen.height;

            // Clamp mouse position to inside camera viewport (optional, if you want cursor only in viewport)
            float clampedX = Mathf.Clamp(mousePos.x, camX, camX + camWidth);
            float clampedY = Mathf.Clamp(mousePos.y, camY, camY + camHeight);

            // Calculate mouse position normalized inside camera viewport (0 to 1)
            float normalizedX = (clampedX - camX) / camWidth;
            float normalizedY = (clampedY - camY) / camHeight;

            // Map normalized position to canvas local coordinates (assuming pivot at center)
            float canvasWidth = canvas.rect.width;
            float canvasHeight = canvas.rect.height;

            float mappedX = (normalizedX - 0.5f) * canvasWidth;
            float mappedY = (normalizedY - 0.5f) * canvasHeight;

            customCursor.transform.localPosition = new Vector2(mappedX, mappedY);


            if (isVisible)
            {
                customCursor.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;

            }
            else
            {
                customCursor.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (cursorState != preCursorState)
            {
                UpdateCursor();
            }

            Cursor.visible = false;
        }

        private bool IsVisible(string state)
        {
            for (int i = 0; i < cursorVisibility.Count; i++)
            {
                if (cursorVisibility[i].state == state)
                {
                    return cursorVisibility[i].visible;
                }
            }

            return true;
        }

        private void SetCursor(CursorState cursorState)
        {
            this.cursorState = cursorState;
        }

        public void UpdateCursor()
        {
            switch (cursorState)
            {
                case CursorState.None:
                    customCursor.sprite = noneCursor;
                    break;
                case CursorState.Hovering:
                    customCursor.sprite = hoveringCursor;
                    break;
                case CursorState.Down:
                    customCursor.sprite = downCursor;
                    break;
            }
        }

        private void OnStateChanged(string newState)
        {
            currentState = newState;
        }

        private void OnInputTypeChanged(InputType type)
        {
            inputType = type;
        }

        private enum CursorState
        {
            None,
            Hovering,
            Down,
        }

        private class CustomCursorStyle
        {
            public CursorState cursorState;
            public Sprite overrideSprite;
        }

        [System.Serializable]
        private class CursorVisibilty
        {
            [Dropdown("States")] public string state;
            [Dropdown("States")] public bool visible;
        }
    }
}

