﻿using System.Collections;
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

public class AmountCondition : AreaCondition
{
    public FindType typeOfFind;
    public CompareType typeOfCompare;

    public string checkTag;
    public LayerMask layer;
    public Object typeTemplate;
    
    public int amount;
    public int numOfObjects = 0;
    
    public override void InitCondition()
    {
        base.InitCondition();
    }

    public override bool CheckCondition()
    {
        numOfObjects = 0;

        // - Find Types -
        if (typeOfFind == FindType.Tag)
        {
            cols = Physics.OverlapBox(transform.position + triggerArea.center, triggerArea.extents, transform.rotation);
            for (int i = 0; i < cols.Length; i++)
                numOfObjects += cols[i].tag == checkTag ? 1 : 0;
        }

        else if (typeOfFind == FindType.Layer)
        {
            cols = Physics.OverlapBox(transform.position + triggerArea.center, triggerArea.extents, Quaternion.identity, layer << 8);
            numOfObjects = cols.Length;
        }
        else if (typeOfFind == FindType.ByType && typeTemplate != null)
        {
            cols = Physics.OverlapBox(transform.position + triggerArea.center, triggerArea.extents, transform.rotation);
            for (int i = 0; i < cols.Length; i++)
                numOfObjects += cols[i].GetComponentsInChildren(typeTemplate.GetType()).Length;
        }

        // - Compare Types - 
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
