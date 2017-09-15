using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCondition : Condition
{
    public Trigger referenceTrigger;
    public bool isTrue;

    public TriggerCondition() { }
    public override void InitCondition(Trigger trigger)
    {
        base.InitCondition(trigger);
        referenceTrigger.Result.AddListener(delegate { SetTrue(); });
    }

    private void SetTrue()
    {
        Debug.Log("Tigger Condition");
        ConditionMet = true;
    }
}
