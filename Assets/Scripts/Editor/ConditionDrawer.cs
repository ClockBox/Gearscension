using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Condition),true)]
public class ConditionDrawer : PropertyDrawer
{
    float fieldWidth;
    int indent;

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        fieldWidth = (pos.width / 5) - 20;
        indent = 0;
        
        Debug.Log(prop.hasChildren);
        
        EditorGUI.BeginProperty(pos, label, prop);
        {
            EditorGUI.LabelField(new Rect(pos.x + fieldWidth * indent, pos.y, fieldWidth, pos.height), "Condtion"); indent++;
        }
        EditorGUI.EndProperty();
    }
}
