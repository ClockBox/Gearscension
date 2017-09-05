using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Trigger),true)]
[CanEditMultipleObjects]
public class TriggerScriptEditor : Editor
{
    Trigger m_target;
    ReorderableList m_condList;
    SerializedProperty conditionProp;
    SerializedProperty resultProp;
    
    protected ConditionType condType;

    protected virtual void OnEnable()
    {
        m_target = (Trigger)target;
        resultProp = serializedObject.FindProperty("result");
        SetupConditionList();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        {
            DrawDefaultInspector();

            GUILayout.Space(10);
            m_condList.DoLayoutList();

            GUILayout.Space(10);
            EditorGUILayout.PropertyField(resultProp);
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void SetupConditionList()
    {
        conditionProp = serializedObject.FindProperty("conditions");

        m_condList = new ReorderableList(serializedObject, conditionProp, true, true, true, true);
        m_condList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(new Rect(rect), "Conditions");
        };
        m_condList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = m_condList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect),element);
            };
        m_condList.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) =>
            {
                GenericMenu menu = new GenericMenu();
                foreach (ConditionType type in System.Enum.GetValues(typeof(ConditionType)))
                    menu.AddItem(new GUIContent(type.ToString()), false, AddCondition, type);
                menu.ShowAsContext();
            };
    }

    private void AddCondition(object type)
    {
        ConditionType conditionType = (ConditionType)type;
        switch (conditionType)
        {
            case ConditionType.Timed:
                m_target.conditions.Add(new TimedCondition(m_target));
                break;
            case ConditionType.Area:
                m_target.conditions.Add(new AreaCondition(m_target));
                break;
            case ConditionType.Button:
                m_target.conditions.Add(new ButtonCondition(m_target));
                break;
            case ConditionType.Destroyed:
                m_target.conditions.Add(new DestroyedCondition(m_target));
                break;
            case ConditionType.Amount:
                m_target.conditions.Add(new AmountCondition(m_target));
                break;
        }
    }
    private void RemoveCondition(int index)
    {
        m_target.conditions.RemoveAt(index);
    }
}
