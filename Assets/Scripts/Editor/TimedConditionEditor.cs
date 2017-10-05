using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(TimedCondition),true)]
//public class TimerConditionEditor : ConditionEditor
//{
//    SerializedProperty timerAmount;
//    SerializedProperty loop;

//    protected override void OnEnable()
//    {
//        timerAmount = serializedObject.FindProperty("timerAmount");
//        loop = serializedObject.FindProperty("loop");
//    }

//    protected override void DrawCondition()
//    {
//    }
//}

[CustomPropertyDrawer(typeof(TimedCondition), true)]
public class TimerDrawer : ConditionDrawer
{
    protected override void DrawCondition(Rect pos, SerializedProperty prop, GUIContent label)
    {
        Debug.Log("TimerDrawer");
        EditorGUILayout.LabelField("Condition");
    }
}
