using Unity.Cinemachine;
using UnityEngine;

namespace TemplateTools
{
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(CinemachineBrain))]
    public class LetterboxWithCinemachine : MonoBehaviour
    {
        [Tooltip("Target aspect ratio, e.g. 16:9 => 1.7777")]
        public float targetAspect = 16f / 9f;

        private Camera _camera;
        private CinemachineBrain _brain;

        private int _lastScreenWidth;
        private int _lastScreenHeight;
        private float _lastTargetAspect;

        private void OnEnable()
        {
            _camera = GetComponent<Camera>();
            _brain = GetComponent<CinemachineBrain>();
            ApplyLetterbox();
        }

        private void OnValidate()
        {
            if (_camera == null) _camera = GetComponent<Camera>();
            if (_brain == null) _brain = GetComponent<CinemachineBrain>();
            ApplyLetterbox();
        }

        private void LateUpdate()
        {
            if (Screen.width != _lastScreenWidth ||
                Screen.height != _lastScreenHeight ||
                targetAspect != _lastTargetAspect)
            {
                ApplyLetterbox();
            }
        }

        private void ApplyLetterbox()
        {
            float windowAspect = (float)Screen.width / Screen.height;
            float scaleHeight = windowAspect / targetAspect;

            Rect rect = _camera.rect;

            if (scaleHeight < 1f)
            {
                // Letterbox (black bars top & bottom)
                rect.width = 1f;
                rect.height = scaleHeight;
                rect.x = 0f;
                rect.y = (1f - scaleHeight) / 2f;
            }
            else
            {
                // Pillarbox (black bars left & right)
                float scaleWidth = 1f / scaleHeight;
                rect.width = scaleWidth;
                rect.height = 1f;
                rect.x = (1f - scaleWidth) / 2f;
                rect.y = 0f;
            }

            _camera.rect = rect;

            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;
            _lastTargetAspect = targetAspect;
        }
    }
}