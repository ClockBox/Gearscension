using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionType
{
    Timed,                          // - Trigger is simply placed on a timer with an option to loop.
    Area,                           // – Triggered when "CheckObject" is within the trigger's area.
    Destroyed,                      // - Triggered when a specific "CheckObject" is destroyed.
    Amount,                         // - Triggered when defined amount of “objects” are in scene(can be zero).  
    Button,                         // - Triggered with button press.
    Trigger                         // - Triggered when referenced Trigger returns true.
}

[System.Serializable]
[RequireComponent(typeof(Trigger))]
public class Condition : MonoBehaviour
{
    protected Trigger trigger;
    public Trigger Trigger
    {
        get { return trigger; }
        set { trigger = value; }
    }

    [Space(20)]
    protected bool conditionIsMet = false;
    public bool ConditionMet
    {
        get { return conditionIsMet; }
        set { conditionIsMet = value; }
    }

    protected void OnEnable()
    {
        trigger = GetComponent<Trigger>();
        CheckVisible();
        InitCondition();
    }

    protected void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Reset()
    {
        trigger = GetComponent<Trigger>();
        trigger.Conditions.Add(this);
        CheckVisible();
    }

    public void CheckVisible()
    {
        if (trigger && trigger.Conditions.Contains(this))
            hideFlags = HideFlags.HideInInspector;
        else hideFlags = HideFlags.None;
    }

    public virtual void InitCondition() { }
    public virtual void ResetCondition() { conditionIsMet = false; }
    public virtual bool checkCondition() { return false; }
}