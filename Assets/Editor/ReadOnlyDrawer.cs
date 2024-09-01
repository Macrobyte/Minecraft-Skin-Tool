using UnityEditor;
using UnityEngine;
using CustomAttributes;

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Save the current GUI enabled state
            bool previousGUIState = GUI.enabled;

            // Disable the GUI (makes the field read-only)
            GUI.enabled = false;

            // Draw the property in its disabled state
            EditorGUI.PropertyField(position, property, label, true);

            // Restore the previous GUI enabled state
            GUI.enabled = previousGUIState;
        }
    }
#endif




