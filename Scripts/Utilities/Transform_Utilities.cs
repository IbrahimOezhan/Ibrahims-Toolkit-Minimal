using System.Collections.Generic;
using UnityEngine;

namespace IbrahKit
{
    public class Transform_Utilities
    {
        public static List<T> GetChildren<T>(Transform tr)
        {
            List<T> elements = new();
            foreach (Transform child in tr)
            {
                T[] compArray = child.GetComponents<T>();

                foreach (T comp in compArray)
                {
                    elements.Add(comp);
                }

                if (child.childCount > 0) elements.AddRange(GetChildren<T>(child));
            }
            return elements;
        }

        public static T GetParent<T>(Transform tr)
        {
            if (tr.parent != null)
            {
                if (tr.parent.TryGetComponent<T>(out var element)) return element;
                else return GetParent<T>(tr.parent);
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

        public static void SortTransformsOfParent(List<GameObject> children)
        {
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