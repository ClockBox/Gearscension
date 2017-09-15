using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCondition : Condition
{
    public ButtonTrigger button;
    public bool Toggled; 

    public ButtonCondition() { }
    public override void InitCondition(Trigger trigger)
    {
        trigger.StartCoroutine(inputCheck());
    }

    protected virtual IEnumerator inputCheck()
    {
        while(true)
        {
            if (trigger.InArea && Input.GetButtonDown("Action"))
            {
                conditionIsMet = true;
                break;
            }
            else conditionIsMet = false;
            yield return null;
        }
    }
}