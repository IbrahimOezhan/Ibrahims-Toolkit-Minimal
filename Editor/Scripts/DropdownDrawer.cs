using System.Collections.Generic;
using System.IO;
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
            string filePath = dropdownAttribute.filePath;

            if (!File.Exists(filePath))
            {
                EditorGUI.LabelField(position, label.text, "File doesn't exist");
                return;
            }

            List<string> dropdownInput = String_Utilities.GetDropdown(filePath);

            if (dropdownInput == null || dropdownInput.Count == 0)
            {
                EditorGUI.LabelField(position, label.text, "No options");
                return;
            }

            int selectedIndex = Mathf.Max(0, dropdownInput.IndexOf(property.stringValue));
            selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, dropdownInput.ToArray());
            property.stringValue = dropdownInput[selectedIndex];
        }
    }
}
