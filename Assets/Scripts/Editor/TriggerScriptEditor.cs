using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Trigger),true)]
[CanEditMultipleObjects]
public class TriggerScriptEditor : Editor
{
    Trigger m_target;

    public enum ConditionTypes
    {
        NewCondition,
        Timed,                          // - Trigger is simply placed on a timer with an option to loop.
        Area,                           // – Triggered when "CheckObject" is within the trigger's area.
        Button,                         // - Triggered when "CheckObject" is in area and "Input" is recieved 
        Destroyed,                      // - triggered when a specific "CheckObject" or "objectType" is destroyed.
        Amount                          // - Triggered when defined amount of “objectType” are in scene(can be zero).
    }
    protected ConditionTypes condType;

    protected virtual void OnEnable()
    {
        m_target = (Trigger)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        if (m_target.conditions.Count > 0)
        {
            EditorGUILayout.LabelField("Conditions:");
            for (int i = 0; i < m_target.conditions.Count; i++)
                DrawCondition(i);
        }

        EditorGUI.BeginChangeCheck();
        condType = (ConditionTypes)EditorGUILayout.EnumPopup("Condition Type", condType);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(m_target, "Add Condiction");
            AddCondition(condType);
            condType = ConditionTypes.NewCondition;
            EditorUtility.SetDirty(m_target);
        }

        serializedObject.ApplyModifiedProperties();
    }
    
    protected virtual void DrawCondition(int index)
    {
        if (index < 0 || index > m_target.conditions.Count)
            return;
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(20);
            SerializedProperty conditionProp = serializedObject.FindProperty("conditions").GetArrayElementAtIndex(index);
            Debug.Log(conditionProp.type);
            EditorGUILayout.PropertyField(conditionProp, true);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                Undo.RecordObject(m_target, "Remove Condition");
                RemoveCondition(index);
                EditorUtility.SetDirty(m_target);
            }
        }
        GUILayout.EndHorizontal();
    }

    private void AddCondition(ConditionTypes conditionType)
    {
        switch (conditionType)
        {
            case ConditionTypes.Timed:
                m_target.conditions.Add(new TimedCondition(m_target,"Timed", m_target.player));
                break;
            case ConditionTypes.Area:
                m_target.conditions.Add(new AreaCondition(m_target, "Area", m_target.player));
                break;
            case ConditionTypes.Button:
                m_target.conditions.Add(new ButtonCondition(m_target, "Button", m_target.player));
                break;
            case ConditionTypes.Destroyed:
                m_target.conditions.Add(new DestroyedCondition(m_target, "Destroyed", m_target.player));
                break;
            case ConditionTypes.Amount:
                m_target.conditions.Add(new AmountCondition(m_target, "Amount", m_target.player));
                break;
        }
    }
    private void RemoveCondition(int index)
    {
        m_target.conditions.RemoveAt(index);
    }
}
