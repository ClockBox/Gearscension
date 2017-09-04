using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Condition),true)]
public class ConditionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        float fieldWidth = (pos.width / 6) - 20;
        int indent = 0;

        EditorGUI.BeginProperty(pos, label, prop);
        {
            SerializedProperty nameProp = prop.FindPropertyRelative("name");
            SerializedProperty objectProp = prop.FindPropertyRelative("checkObject");

            EditorGUI.LabelField(new Rect(pos.x + fieldWidth * indent, pos.y, fieldWidth, pos.height), nameProp.stringValue); indent++;
            EditorGUI.LabelField(new Rect(pos.x + fieldWidth * indent, pos.y, fieldWidth, pos.height), "Object"); indent++;
            EditorGUI.PropertyField(new Rect(pos.x + fieldWidth * indent, pos.y, fieldWidth, pos.height), objectProp, GUIContent.none); indent++;
            DrawCondtion(pos, prop, label);
        }
        EditorGUI.EndProperty();
    }

    protected virtual void DrawCondtion(Rect pos, SerializedProperty prop, GUIContent label) { }
}
