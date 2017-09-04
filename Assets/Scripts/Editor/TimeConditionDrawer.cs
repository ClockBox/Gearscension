using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TimedCondition),true)]
public class TimeConditionDrawer : ConditionDrawer
{
    protected override void DrawCondtion(Rect pos, SerializedProperty prop, GUIContent label)
    {
        Debug.Log("herE");
        base.DrawCondtion(pos, prop, label);
    }
}
