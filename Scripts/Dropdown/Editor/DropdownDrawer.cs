#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TemplateTools
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

            List<string> dropdownOptions = new List<string>();

            // Read strings from the specified text file
            string path = dropdownAttribute.filePath;

            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                dropdownOptions.AddRange(lines);
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "File not found");
                return;
            }

            if (dropdownOptions.Count > 0)
            {
                int selectedIndex = Mathf.Max(0, dropdownOptions.IndexOf(property.stringValue));
                selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, dropdownOptions.ToArray());
                property.stringValue = dropdownOptions[selectedIndex];
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "No options available");
            }
        }
    }
}

#endif