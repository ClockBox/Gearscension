using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCondition : Condition
{
    public ButtonCondition(Trigger trigger) : base(trigger)
    {
        trigger.StartCoroutine(inputCheck());
    }

    protected virtual IEnumerator inputCheck()
    {
        while(true)
        {
            if (trigger.InArea)
            {
                if (Input.GetButtonDown("Action"))
                {
                    conditionIsMet = true;
                    break;
                }
                else
                    conditionIsMet = false;
            }
            yield return null;
        }
    }
}