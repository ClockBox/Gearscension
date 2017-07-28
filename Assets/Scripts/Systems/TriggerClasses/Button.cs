using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Condition
{
    public Button(Trigger trigger, string name, object checkObject, GameObject player) : base (trigger, name, checkObject, player)
    {

    }

    protected virtual IEnumerator inputCheck()
    {
        while(trigger.inArea)
        {
            if (Input.GetButtonDown("Action"))
            {
                conditionIsMet = true;
                break;
            }
            else
                conditionIsMet = false;
            yield return null;
        }
    }
}