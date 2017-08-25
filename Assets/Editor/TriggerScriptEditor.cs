using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Trigger))]
public class TriggerScriptEditor : Editor
{
    ConditionTypes cond;
    Trigger myTarget;

    public enum ConditionTypes
    {
        None,
        Timed,                          // - Trigger is simply placed on a timer with an option to loop.
        InArea,                         // – Triggered when "CheckObject" is within the trigger's area.
        Button,                         // - Triggered when "CheckObject" is in area and "Input" is recieved 
        ObjectDestroyed,                // - triggered when a specific "CheckObject" or "objectType" is destroyed.
        AmountOf                        // - Triggered when defined amount of “objectType” are in scene(can be zero).
    }
    
    public override void OnInspectorGUI()
    {
        myTarget = (Trigger)target;

        cond = (ConditionTypes)EditorGUILayout.EnumPopup("Condition Type", cond);
        AddCondition(cond);
    }
    
    void AddCondition(ConditionTypes conditionType)
    {
        switch (conditionType)
        {
            case ConditionTypes.Timed:
                myTarget.conditions.Add(new Timed(myTarget, "Timed", myTarget.player));
                break;
            case ConditionTypes.InArea:
                myTarget.conditions.Add(new InArea(myTarget, "In Area", myTarget.player));
                break;
            case ConditionTypes.Button:
                myTarget.conditions.Add(new Button(myTarget, "Button", myTarget.player));
                break;
            case ConditionTypes.ObjectDestroyed:
                myTarget.conditions.Add(new ObjectDestroyed(myTarget, "Object Destroyed", myTarget.player));
                break;
            case ConditionTypes.AmountOf:
                myTarget.conditions.Add(new AmountOf(myTarget, "Amount Of", myTarget.player));
                break;
        }
    }
}
