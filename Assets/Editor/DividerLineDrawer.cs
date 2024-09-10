using UnityEditor;
using CustomAttributes;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(DividerAttribute))]
public class DividerLineDrawer : DecoratorDrawer
{
    public override void OnGUI(Rect position)
    {
        DividerAttribute lineAttribute = (DividerAttribute)attribute;

        // Save original color
        Color oldColor = GUI.color;

        // Set color and draw line
        GUI.color = lineAttribute.color;
        EditorGUI.DrawRect(new Rect(position.x, position.y + 1, position.width, lineAttribute.thickness), lineAttribute.color);

        // Reset color
        GUI.color = oldColor;
    }
}
#endif
