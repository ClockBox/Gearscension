using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedCondition : Condition
{
    public Object checkObject;
    public bool isAlive;
    
    public override bool checkCondition()
    {
        return conditionIsMet = (isAlive && checkObject != null) || (!isAlive && checkObject == null);
    }
}
