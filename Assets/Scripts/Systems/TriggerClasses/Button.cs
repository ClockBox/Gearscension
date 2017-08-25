using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Condition
{
    public Button(Trigger trigger, string name, GameObject player) : base (trigger, name, player)
    {
        trigger.StartCoroutine(inputCheck());
    }

    protected virtual IEnumerator inputCheck()
    {
        while(true)
        {
            if (trigger.inArea)
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
}