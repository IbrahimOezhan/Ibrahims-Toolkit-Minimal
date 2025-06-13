using Sirenix.OdinInspector;
using UnityEngine;

namespace IbrahKit
{
    [System.Serializable]
    public class Custom_Menu_Item : Menu_Item
    {
        [SerializeField] private Vector2 position;
        [SerializeField] private Vector2 xAnchor;
        [SerializeField] private Vector2 yAnchor;
        [SerializeField] private Vector2 pivot;

        public void OnDrawGizmos(Canvas canvas)
        {
            if (canvas == null)
                return;

            RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
            if (canvasRectTransform == null)
                return;

            Vector2 canvasSize = canvasRectTransform.rect.size;

            // Calculate anchor positions in canvas local space
            Vector2 anchorMin = new Vector2(xAnchor.x, yAnchor.x) * canvasSize;
            Vector2 anchorMax = new Vector2(xAnchor.y, yAnchor.y) * canvasSize;

            Vector2 rectSize = anchorMax - anchorMin;

            Vector2 pivotOffset = new Vector2(pivot.x * rectSize.x, pivot.y * rectSize.y);

            // Rect local position inside canvas
            Vector2 rectLocalPos = anchorMin + position - pivotOffset;

            // Get canvas space corners in local space
            Vector3 bottomLeft = new Vector3(rectLocalPos.x, rectLocalPos.y, 0);
            Vector3 bottomRight = bottomLeft + new Vector3(rectSize.x, 0, 0);
            Vector3 topRight = bottomLeft + new Vector3(rectSize.x, rectSize.y, 0);
            Vector3 topLeft = bottomLeft + new Vector3(0, rectSize.y, 0);

            // Convert these points to world space depending on canvas render mode
            Vector3[] worldCorners = new Vector3[4];

            switch (canvas.renderMode)
            {
                case RenderMode.ScreenSpaceOverlay:
                    {
                        // For overlay, canvas space = screen space
                        // Convert screen position to world via camera (null for overlay)
                        Camera cam = null;

                        // Convert screen points to world points on the near plane (z=0)
                        worldCorners[0] = ScreenToWorld(canvas, bottomLeft, cam);
                        worldCorners[1] = ScreenToWorld(canvas, bottomRight, cam);
                        worldCorners[2] = ScreenToWorld(canvas, topRight, cam);
                        worldCorners[3] = ScreenToWorld(canvas, topLeft, cam);
                    }
                    break;
                case RenderMode.ScreenSpaceCamera:
                    {
                        Camera cam = canvas.worldCamera;
                        worldCorners[0] = ScreenToWorld(canvas, bottomLeft, cam);
                        worldCorners[1] = ScreenToWorld(canvas, bottomRight, cam);
                        worldCorners[2] = ScreenToWorld(canvas, topRight, cam);
                        worldCorners[3] = ScreenToWorld(canvas, topLeft, cam);
                    }
                    break;
                case RenderMode.WorldSpace:
                    {
                        // Canvas is in world space, so just transform points from local space of canvas RectTransform to world
                        worldCorners[0] = canvasRectTransform.TransformPoint(bottomLeft);
                        worldCorners[1] = canvasRectTransform.TransformPoint(bottomRight);
                        worldCorners[2] = canvasRectTransform.TransformPoint(topRight);
                        worldCorners[3] = canvasRectTransform.TransformPoint(topLeft);
                    }
                    break;
            }

            Gizmos.color = Color.green;
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawLine(worldCorners[i], worldCorners[(i + 1) % 4]);
            }
        }

        private Vector3 ScreenToWorld(Canvas canvas, Vector3 canvasLocalPos, Camera cam)
        {
            // Canvas local pos is relative to bottom-left of canvas (0,0)
            // Convert to screen position:
            Vector2 screenPoint = canvasLocalPos;

            // Canvas size
            RectTransform rt = canvas.GetComponent<RectTransform>();
            Vector2 canvasSize = rt.rect.size;

            // Screen space is bottom-left origin for GUI
            // So just use screenPoint as is (in pixels)
            // But to be safe clamp inside camera viewport rect

            if (cam != null)
            {
                // Clamp screenPoint to camera viewport in pixels
                Rect vp = cam.pixelRect;
                screenPoint.x = Mathf.Clamp(screenPoint.x, vp.xMin, vp.xMax);
                screenPoint.y = Mathf.Clamp(screenPoint.y, vp.yMin, vp.yMax);

                Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, cam.nearClipPlane));
                return worldPos;
            }
            else
            {
                // Overlay mode: Screen point to world space on XY plane at z=0
                Vector3 worldPos = new Vector3(screenPoint.x, screenPoint.y, 0);
                return worldPos;
            }
        }

        public void SetRectTransform(RectTransform rectTransform)
        {
            rectTransform.localPosition = position;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.anchorMin = new(xAnchor.x, yAnchor.x);
            rectTransform.anchorMax = new(xAnchor.y, yAnchor.y);
            rectTransform.pivot = pivot;
        }
    }
}

