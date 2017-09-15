using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FindType
{
    Tag,
    Layer,
    ByType
}

public enum CompareType
{
    LessThan,
    GreaterThan,
    EqualTo,
    LessThanOrEqual,
    GreaterThanOrEqual,
}

public class AmountCondition : Condition
{
    public string tag;
    public LayerMask layer;

    public Object typeTemplate;

    public FindType typeOfFind;
    public CompareType typeOfCompare;
    public Vector3 areaSize;
    public int amount;
    Collider[] colArray;

    public AmountCondition() { }
    public override void InitCondition(Trigger trigger)
    {
        base.InitCondition(trigger);
        areaSize = trigger.GetComponent<Collider>().bounds.extents;
        trigger.StartCoroutine(CheckArea());
    }

    IEnumerator CheckArea()
    {
        int numOfObjects = 0;
        while (true)
        {
            if (typeOfFind == FindType.Layer)
            {
                colArray = Physics.OverlapBox(trigger.transform.position, areaSize, Quaternion.identity, layer << 8);
                numOfObjects = colArray.Length;
            }
            else if (typeOfFind == FindType.Tag)
                numOfObjects = GameObject.FindGameObjectsWithTag(tag).Length;

            else if (typeOfFind == FindType.ByType && typeTemplate != null)
                numOfObjects = Object.FindObjectsOfType(typeTemplate.GetType()).Length;

            if (typeOfCompare == CompareType.LessThan && numOfObjects < amount)
                ConditionMet = true;

            else if (typeOfCompare == CompareType.GreaterThan && numOfObjects > amount)
                ConditionMet = true;

            else if (typeOfCompare == CompareType.EqualTo && numOfObjects == amount)
                ConditionMet = true;

            else if (typeOfCompare == CompareType.LessThanOrEqual && numOfObjects <= amount)
                ConditionMet = true;

            else if (typeOfCompare == CompareType.GreaterThanOrEqual && numOfObjects >= amount)
                ConditionMet = true;

            else conditionIsMet = false;

            yield return null;
        }
    }
}
