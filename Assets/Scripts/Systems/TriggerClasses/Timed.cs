using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timed : Condition
{
    [SerializeField]
    float timerAmount;
    
    public Timed(Trigger trigger, string name, GameObject player) : base (trigger, name, player)
    {
        trigger.StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(this.timerAmount);
        conditionIsMet = true;
    }
}