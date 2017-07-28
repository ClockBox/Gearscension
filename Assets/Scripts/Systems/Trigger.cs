using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Idealy ConditionTypes would be moved to a custom inspector using an EnumMaskField: https://docs.unity3d.com/ScriptReference/EditorGUILayout.EnumMaskField.html
// and would not need to be included in the final build.
// Selecting a type would then create a new condition and add it to the trigger conditions list
public enum ConditionTypes
{
    Timed,                          // - Trigger is simply placed on a timer with an option to loop.
    InArea,                         // – Triggered when "CheckObject" is within the trigger's area.
    Button,                         // - Triggered when "CheckObject" is in area and "Input" is recieved 
    ObjectDestroyed,                // - triggered when a specific "CheckObject" or "objectType" is destroyed.
    AmountOf                        // - Triggered when defined amount of “objectType” are in scene(can be zero).
}

[RequireComponent(typeof(Collider))]
public class Trigger : MonoBehaviour
{
    public ConditionTypes conditionType;
    
    [SerializeField]
    public Condition condish;
    public GameObject player;
    public Trigger t;
    public bool inArea;

    [SerializeField]
    UnityEvent result;              // - Triggers an event when conditions are met.

    List<Condition> conditions = new List<Condition>();

    public void Start()
    {
        condish = new Timed(this, "TimedTrigger", t, player,2.0f);
    }

    public virtual void CheckConditions()
    {
        if (conditions.Count > 0)
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                if (conditions[i].ConditionMet == false)
                    return;
            }
            result.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            inArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            inArea = false;
        }
    }
}