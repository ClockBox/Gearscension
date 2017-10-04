using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Trigger),true)]
[CanEditMultipleObjects]
public class TriggerEditor : Editor
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
            
            Condition[] temp = m_target.GetComponents<Condition>();
            for (int i = 0; i < temp.Length; i++)
                temp[i].CheckVisible();
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void SetupConditionList()
    {
        conditionProp = serializedObject.FindProperty("conditions");

        m_condList = new ReorderableList(serializedObject, conditionProp, true, true, true, true);
        m_condList.elementHeight = 45;
        m_condList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Conditions");
        };
        m_condList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            SerializedProperty element = m_condList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element, true);
        };
        m_condList.onAddDropdownCallback = (Rect buttonRect, ReorderableList list) =>
        {
            GenericMenu menu = new GenericMenu();
            foreach (ConditionType type in System.Enum.GetValues(typeof(ConditionType)))
                menu.AddItem(new GUIContent(type.ToString()), false, AddCondition, type);
            menu.ShowAsContext();
        };
        m_condList.onRemoveCallback = (ReorderableList list) =>
        {
            RemoveCondition(list.index);
        };
    }

    private void AddCondition(object type)
    {
        switch ((ConditionType)type)
        {
            case ConditionType.Timed:
                m_target.gameObject.AddComponent<TimedCondition>();
                break;
            case ConditionType.Area:
                m_target.gameObject.AddComponent<AreaCondition>();
                break;
            case ConditionType.Destroyed:
                m_target.gameObject.AddComponent<DestroyedCondition>();
                break;
            case ConditionType.Amount:
                m_target.gameObject.AddComponent<AmountCondition>();
                break;
            case ConditionType.Button:
                m_target.gameObject.AddComponent<ButtonCondition>();
                break;
            case ConditionType.Trigger:
                m_target.gameObject.AddComponent<TriggerCondition>();
                break;
        }
    }
    private void RemoveCondition(int index)
    {
        Condition temp = m_target.Conditions[index];
        m_target.Conditions.RemoveAt(index);
        DestroyImmediate(temp);
    }
}
