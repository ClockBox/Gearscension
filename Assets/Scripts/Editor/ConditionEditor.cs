using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(Condition),true)]
//public class ConditionEditor : Editor
//{
//    protected Condition m_target;

//    protected virtual void OnEnable()
//    {
//        m_target = (Condition)target;
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();
//        {
//            DrawDefaultInspector();
//            DrawCondition();
//        }
//        serializedObject.ApplyModifiedProperties();
//    }

//    protected virtual void DrawCondition()
//    {
//        EditorGUILayout.LabelField("Condition");
//    }
//}

[CustomPropertyDrawer(typeof(Condition), true)]
public class ConditionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect pos,SerializedProperty prop, GUIContent label)
    {
        Debug.Log(prop.hasChildren);

        EditorGUI.BeginProperty(pos,label, prop);
        {
            DrawCondition(pos, prop, label);
        }
        EditorGUI.EndProperty();
    }

    protected virtual void DrawCondition(Rect pos, SerializedProperty prop, GUIContent label)
    {
        Debug.Log("ConditionDrawer");
        EditorGUI.LabelField(pos, "Condition");
    }
}
