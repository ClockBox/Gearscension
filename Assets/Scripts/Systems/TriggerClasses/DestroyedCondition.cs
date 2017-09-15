using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedCondition : Condition
{
    public Object checkObject;
    public bool isAlive;

    public DestroyedCondition() { }
    public override void InitCondition(Trigger trigger)
    {
        base.InitCondition(trigger);
        trigger.StartCoroutine(check());
    }

    private IEnumerator check()
    {
        while (true)
        {
            if ((isAlive && checkObject != null) || !isAlive && checkObject == null)
                ConditionMet = true;
            yield return null;
        }
    }
}
