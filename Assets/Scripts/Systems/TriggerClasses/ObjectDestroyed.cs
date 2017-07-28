using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyed : Condition
{
    [SerializeField]
    GameObject obj;

    public ObjectDestroyed(Trigger trigger, string name, object checkObject, GameObject player) : base (trigger, name, checkObject, player)
    {

    }

    void check()
    {
        conditionIsMet = checkObject == null;
    }
}
