using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace TemplateTools
{
    [RequireComponent(typeof(Camera))]
    public class OverlayAssign : MonoBehaviour
    {
        private Camera overlayCam;

        [SerializeField] private int priority;

        private void Awake()
        {
            overlayCam = GetComponent<Camera>();

            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void Start()
        {
            Assign();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= SceneLoaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Assign();
        }

        private void Assign()
        {
            Camera cam = Camera.main;
            UniversalAdditionalCameraData baseCameraData = cam.GetUniversalAdditionalCameraData();

            if (!baseCameraData.cameraStack.Contains(overlayCam))
            {
                overlayCam.allowMSAA = cam.allowMSAA;
                overlayCam.allowHDR = cam.allowHDR;
                overlayCam.targetDisplay = cam.targetDisplay;
                overlayCam.rect = cam.rect;
                overlayCam.clearFlags = CameraClearFlags.Depth;
                overlayCam.targetTexture = null;

                baseCameraData.cameraStack.Add(overlayCam);

                baseCameraData.cameraStack.Sort((a, b) =>
                {
                    if (a.TryGetComponent(out OverlayAssign overlayA))
                    {
                        if (b.TryGetComponent(out OverlayAssign overlayB))
                        {
                            return overlayA.priority.CompareTo(overlayB.priority);
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        return 0;
                    }
                });
            }
        }
    }
}