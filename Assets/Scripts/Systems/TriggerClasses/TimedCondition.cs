using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimedCondition : Condition
{
    public float timerAmount = 1;
    public bool loop;
    
    private bool isCounting;

    IEnumerator StartTimer()
    {
        isCounting = true;
        Debug.Log("Timer Started");
        yield return new WaitForSeconds(timerAmount);
        Debug.Log("Timer Done");
        ConditionMet = true;
        if (loop)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            isCounting = false;
        }
    }

    public override bool CheckCondition()
    {
        Debug.Log("Timer checked");
        if (!isCounting)
        {
            StartCoroutine(StartTimer());
            ConditionMet = false;
        }
        return conditionIsMet;
    }

    public override void ResetCondition()
    {
        base.ResetCondition();
        StopAllCoroutines();
        isCounting = false;
    }
}