using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimedCondition : Condition
{
    [SerializeField]
    private float timerAmount;

    [SerializeField]
    private bool loop;

    public TimedCondition(Trigger trigger) : base(trigger)
    {
        trigger.StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timerAmount);
        trigger.CheckConditions();
        if (loop) trigger.StartCoroutine(StartTimer());
    }
}