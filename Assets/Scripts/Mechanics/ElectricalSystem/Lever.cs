using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : ElectricalSwitch
{
    public Transform standPoint;
    public Transform handPoint;

    public float delayAmount;

    public override void Activate()
    {
        StartCoroutine(ActivateAfterDelay());
    }

    private IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(delayAmount);
        base.Activate();
        OnActivate.Invoke();
    }
}
