using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Condition
{
    protected static GameObject player;
    protected string name;
    protected Trigger trigger;

    [SerializeField]
    protected object checkObject;
    protected System.Type objectType { get { return checkObject.GetType(); } }

    [Space(20)]
    protected bool conditionIsMet = false;
    public bool ConditionMet                        //Check by the trigger.
    {
        get { return conditionIsMet; }
        set
        {
            conditionIsMet = value;
            if (conditionIsMet == true)
                trigger.CheckConditions();          //If this condition is met then check the trigger to see if the other condtions are as well.
        }
    }

    // - Constuctor
    protected Condition(Trigger trigger,string name,object checkObject, GameObject _player)
    {
        this.name = name;
        this.checkObject = checkObject;
        this.trigger = trigger;

        if (player == null)
        {
            player = _player; 
        }
    }
}