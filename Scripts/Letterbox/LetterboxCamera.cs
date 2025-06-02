using UnityEngine;

namespace IbrahKit
{
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class LetterboxCamera : MonoBehaviour
    {
        [Tooltip("Target aspect ratio, e.g., 16:9 => 1.7777")]
        public float targetAspect = 16f / 9f;

        private Camera _camera;

        private void OnEnable()
        {
            _camera = GetComponent<Camera>();
            UpdateViewport();
        }

        private void OnValidate()
        {
            if (_camera == null) _camera = GetComponent<Camera>();
            UpdateViewport();
        }

        private void Update()
        {
            if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight)
            {
                UpdateViewport();
            }
            else
            {
                UpdateViewport();
            }
        }

        private int _lastScreenWidth;
        private int _lastScreenHeight;

        private void UpdateViewport()
        {
            float windowAspect = (float)Screen.width / Screen.height;
            float scaleHeight = windowAspect / targetAspect;

            Rect rect = _camera.rect;

            if (scaleHeight < 1f)
            {
                // Add letterbox (horizontal bars)
                rect.width = 1f;
                rect.height = scaleHeight;
                rect.x = 0f;
                rect.y = (1f - scaleHeight) / 2f;
            }
            else
            {
                // Add pillarbox (vertical bars)
                float scaleWidth = 1f / scaleHeight;
                rect.width = scaleWidth;
                rect.height = 1f;
                rect.x = (1f - scaleWidth) / 2f;
                rect.y = 0f;
            }

            _camera.rect = rect;

            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
        }
    }
}