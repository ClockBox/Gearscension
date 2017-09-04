using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CompareType
{
    LessThan,
    GreaterThan,
    EqualTo
}

public class AmountCondition : Condition
{
    [SerializeField]
    CompareType typeOfCompare;
    [SerializeField]
    protected Vector3 areaSize;
    [SerializeField]
    protected int layer;
    [SerializeField]
    protected int amountOfObjects;
    Collider[] colArray;

    public AmountCondition(Trigger trigger,string name, GameObject player) : base(trigger, name, player)
    {
        trigger.StartCoroutine(CheckArea());
    }

    IEnumerator CheckArea()
    {
        int numOfObjects;
        while (true)
        {
            colArray = Physics.OverlapBox(trigger.transform.position, areaSize, Quaternion.identity, LayerMask.GetMask(LayerMask.LayerToName(layer)));
            numOfObjects = colArray.Length;

            if (typeOfCompare == CompareType.LessThan && numOfObjects < amountOfObjects)
            {
                conditionIsMet = true;
            }
            else if (typeOfCompare == CompareType.GreaterThan && numOfObjects > amountOfObjects)
            {
                conditionIsMet = true;
            }
            else if (typeOfCompare == CompareType.EqualTo && numOfObjects == amountOfObjects)
            {
                conditionIsMet = true;
            }
            else
                conditionIsMet = false;

            yield return null;
        }
    }
}
