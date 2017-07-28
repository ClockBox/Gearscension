using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmountOf : Condition
{
    [SerializeField]
    protected Vector3 areaSize;
    [SerializeField]
    protected int layer;
    [SerializeField]
    protected int amountOfObjects;

    AmountOf(Trigger trigger, string name, object checkObject, GameObject player) : base(trigger, name, checkObject, player)
    {

    }

    void checkArea()
    {
        Collider[] colArray = Physics.OverlapBox(trigger.transform.position, areaSize, Quaternion.identity, LayerMask.GetMask(LayerMask.LayerToName(layer)));
        amountOfObjects = colArray.Length;
    }
}
