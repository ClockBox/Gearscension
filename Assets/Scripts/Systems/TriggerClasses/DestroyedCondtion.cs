using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedCondition : Condition
{
    [SerializeField]
    GameObject checkObject;

    public DestroyedCondition(Trigger trigger) : base(trigger)
    {
        check();
    }

    void check()
    {
        conditionIsMet = checkObject == null;
    }
}
