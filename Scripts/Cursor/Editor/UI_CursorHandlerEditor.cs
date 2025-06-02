using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace IbrahKit
{

    [CustomEditor(typeof(UI_CursorHandler))]
    public class UI_CursorHandlerEditor : GraphicEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_RaycastTarget"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}