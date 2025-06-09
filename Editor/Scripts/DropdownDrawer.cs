using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IbrahKit
{
    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.targetObjects.Length > 1)
            {
                EditorGUI.LabelField(position, label.text, "Multi-editing is not supported");
                return;
            }

            DropdownAttribute dropdownAttribute = (DropdownAttribute)attribute;

            List<string> dropdownOptions = new();

            if (dropdownAttribute.error.Item1)
            {
                EditorGUI.LabelField(position, label.text,dropdownAttribute.error.Item2);
                return;

            }

            dropdownOptions.AddRange(dropdownAttribute.dropdownInput);

            int selectedIndex = Mathf.Max(0, dropdownOptions.IndexOf(property.stringValue));
            selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, dropdownOptions.ToArray());
            property.stringValue = dropdownOptions[selectedIndex];
        }
    }
}