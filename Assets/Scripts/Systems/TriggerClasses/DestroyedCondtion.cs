using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedCondition : Condition
{
    [SerializeField]
    GameObject obj;

    public DestroyedCondition(Trigger trigger, string name, GameObject player) : base(trigger, name, player)
    {
        check();
    }

    void check()
    {
        conditionIsMet = checkObject == null;
    }
}
