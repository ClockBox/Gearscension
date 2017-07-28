using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timed : Condition
{
    [SerializeField]
    float timerAmount;
    
    public Timed(Trigger trigger, string name, object checkObject, GameObject player, float timerAmount) : base (trigger, name, checkObject, player)
    {
        this.timerAmount = timerAmount;
    }

    IEnumerator startTimer()
    {
        yield return new WaitForSeconds(this.timerAmount);
        conditionIsMet = true;
    }
}