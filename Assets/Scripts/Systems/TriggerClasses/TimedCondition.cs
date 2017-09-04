using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimedCondition : Condition
{
    [SerializeField]
    float timerAmount;

    public TimedCondition(Trigger trigger, string name, GameObject player) : base(trigger, name, player)
    {
        trigger.StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timerAmount);
        conditionIsMet = true;
    }
}