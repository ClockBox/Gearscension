using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimedCondition : Condition
{
    public float timerAmount = 1;
    public bool loop;

    public TimedCondition() { }

    public override void InitCondition(Trigger trigger)
    {
        base.InitCondition(trigger);
        trigger.StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timerAmount);
        Debug.Log("Timer Done");
        ConditionMet = true;
        yield return new WaitForEndOfFrame();
        conditionIsMet = false;
        if (loop) trigger.StartCoroutine(StartTimer());
    }
}