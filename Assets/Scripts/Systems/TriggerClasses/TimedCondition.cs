using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimedCondition : Condition
{
    public float timerAmount = 1;
    public bool loop;
    
    public override void InitCondition()
    {
        base.InitCondition();
        trigger.StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timerAmount);
        Debug.Log("Timer Done");
        ConditionMet = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        conditionIsMet = false;
        if (loop) trigger.StartCoroutine(StartTimer());
    }

    public override bool checkCondition()
    {
        Debug.Log("Timer checked");
        return conditionIsMet;
    }
}