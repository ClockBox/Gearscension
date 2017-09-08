using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionType
{
    Timed,                          // - Trigger is simply placed on a timer with an option to loop.
    Area,                           // – Triggered when "CheckObject" is within the trigger's area.
    Button,                         // - Triggered when "CheckObject" is in area and "Input" is recieved 
    Destroyed,                      // - triggered when a specific "CheckObject" or "objectType" is destroyed.
    Amount,                         // - Triggered when defined amount of “objectType” are in scene(can be zero).                  
}

[System.Serializable]
public class Condition :ScriptableObject
{
    protected static GameObject player;
    protected Trigger trigger;

    [Space(20)]
    protected bool conditionIsMet = false;
    public bool ConditionMet
    {
        get { return conditionIsMet; }
        set
        {
            conditionIsMet = value;
            if (conditionIsMet == true)
                trigger.CheckConditions();
        }
    }
    
    public Condition(Trigger trigger)
    {
        this.trigger = trigger;

        if (player == null)
            player = trigger.Player;
    }
}