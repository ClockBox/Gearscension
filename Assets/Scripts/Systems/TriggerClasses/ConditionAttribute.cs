using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionAttribute : PropertyAttribute
{
    public ConditionType type;

    public ConditionAttribute(ConditionType type)
    {
        this.type = type;
    }
}
