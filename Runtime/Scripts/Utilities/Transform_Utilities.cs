using System.Collections.Generic;
using UnityEngine;

namespace IbrahKit
{
    public static class Transform_Utilities
    {
        public static List<T> GetComponentsInChildren<T>(Transform transform, bool includeThis = false)
        {
            if (transform == null)
            {
                Debug.LogWarning("Transform is null");
                return new(0);
            }

            List<T> elements = new();

            if (includeThis)
            {
                elements.AddRange(transform.GetComponents<T>());
            }

            foreach (Transform child in transform)
            {
                T[] compArray = child.GetComponents<T>();

                foreach (T comp in compArray)
                {
                    elements.Add(comp);
                }

                if (child.childCount > 0)
                {
                    elements.AddRange(GetComponentsInChildren<T>(child));
                }
            }

            return elements;
        }

        public static T GetParent<T>(Transform transform)
        {
            if (transform.parent != null)
            {
                if (transform.parent.TryGetComponent<T>(out var element))
                {
                    return element;
                }
                else return GetParent<T>(transform.parent);
            }

            return default;
        }

        public static Quaternion GetRotation(Transform _transformToRotate, Transform _rotateTarget, float _offset)
        {
            var _heading = _rotateTarget.position - _transformToRotate.position;
            var _heading2d = new Vector2(_heading.x, _heading.z).normalized;
            var _angle = Mathf.Atan2(_heading2d.y, _heading2d.x) * -Mathf.Rad2Deg + _offset;
            return Quaternion.AngleAxis(_angle, Vector3.up);
        }

        public static void SetRectStretchMode(RectTransform rect)
        {
            // Set to Stretch mode
            rect.anchorMin = new Vector2(0, 0); // Bottom-left
            rect.anchorMax = new Vector2(1, 1); // Top-right

            // Set offset values to zero
            rect.offsetMin = Vector2.zero; // Left, Bottom
            rect.offsetMax = Vector2.zero; // Right, Top
        }

        public static void SortGameObjects(Transform parent)
        {
            List<GameObject> children = new();

            foreach (Transform child in parent)
            {
                children.Add(child.gameObject);
            }

            SortGameObjects(children);
        }

        public static void SortGameObjects(List<GameObject> children)
        {
            if (children == null)
            {
                Debug.LogWarning("List is null");

                return;
            }

            if (children.Count == 0)
            {
                Debug.LogWarning("List is empty");

                return;
            }

            children.Sort((GameObject one, GameObject two) =>
            {
                return one.name.CompareTo(two.name);
            });

            for (int i = 0; i < children.Count; i++)
            {
                children[i].transform.SetSiblingIndex(i);
            }
        }
    }
}