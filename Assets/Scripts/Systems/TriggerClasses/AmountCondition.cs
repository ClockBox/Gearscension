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
    public FindType typeOfFind;
    public CompareType typeOfCompare;

    public string checkTag;
    public LayerMask layer;
    public Object typeTemplate;

    public Vector3 areaSize;
    public int amount;
    private Collider[] colArray;

    public int numOfObjects = 0;
    
    public override void InitCondition()
    {
        base.InitCondition();
        areaSize = trigger.GetComponent<Collider>().bounds.extents;
    }

    public override bool checkCondition()
    {
        if (typeOfFind == FindType.Layer)
        {
            colArray = Physics.OverlapBox(trigger.transform.position, areaSize, Quaternion.identity, layer << 8);
            numOfObjects = colArray.Length;
        }
        else if (typeOfFind == FindType.Tag)
            numOfObjects = GameObject.FindGameObjectsWithTag(checkTag).Length;

        else if (typeOfFind == FindType.ByType && typeTemplate != null)
            numOfObjects = FindObjectsOfType(typeTemplate.GetType()).Length;

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

        return ConditionMet;
    }
}
