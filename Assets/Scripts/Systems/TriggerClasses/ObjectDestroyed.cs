using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyed : Condition
{
    [SerializeField]
    GameObject obj;

    public ObjectDestroyed(Trigger trigger, string name, GameObject player) : base (trigger, name, player)
    {
        check();
    }

    void check()
    {
        conditionIsMet = checkObject == null;
    }
}
