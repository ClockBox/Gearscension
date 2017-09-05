using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TimedCondition),true)]
public class TimerConditionDrawer : ConditionDrawer
{
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        float fieldWidth = (pos.width / 6) - 20;
        int indent = 0;

        EditorGUI.BeginProperty(pos, label, prop);
        {
            EditorGUI.LabelField(new Rect(pos.x + fieldWidth * indent, pos.y, fieldWidth, pos.height), "Timer"); indent++;

            EditorGUI.LabelField(new Rect(pos.x + fieldWidth * indent, pos.y, fieldWidth, pos.height), "Loop"); indent++;
            EditorGUI.PropertyField(new Rect(pos.x + fieldWidth * indent, pos.y, fieldWidth, pos.height), prop.FindPropertyRelative("loop")); indent++;
        }
        EditorGUI.EndProperty();
    }
}
