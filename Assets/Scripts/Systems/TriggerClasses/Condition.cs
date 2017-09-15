using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ConditionType
{
    Timed,                          // - Trigger is simply placed on a timer with an option to loop.
    Area,                           // – Triggered when "CheckObject" is within the trigger's area.
    Destroyed,                      // - Triggered when a specific "CheckObject" is destroyed.
    Amount,                         // - Triggered when defined amount of “objects” are in scene(can be zero).   
    Trigger                         // - Triggered when referenced Trigger returns true.
}

[System.Serializable]
public class Condition : ScriptableObject
{
    protected GameObject gameObject;
    protected Trigger trigger;

    [Space(20)]
    protected bool conditionIsMet = false;
    public bool ConditionMet
    {
        get { return conditionIsMet; }
        set
        {
            conditionIsMet = value;
            if (value == true)
                trigger.CheckConditions();
        }
    }

    public Condition() { }
    public virtual void InitCondition(Trigger trigger)
    {
        this.trigger = trigger;
        gameObject = trigger.gameObject;
        Debug.Log(gameObject);
    }

    public virtual void OnDrawGizmos() { }
    
}