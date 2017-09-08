using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCondition : Condition
{
    [SerializeField]
    protected GameObject checkObject;
    [SerializeField]
    protected int layer;

    Vector3 triggerArea;
    Collider[] cols;

    public AreaCondition(Trigger trigger) : base(trigger)
    {
        triggerArea = trigger.GetComponent<Collider>().bounds.extents;
        trigger.StartCoroutine(CheckArea());
    }

    IEnumerator CheckArea()
    {
        while (true)
        {
            cols = Physics.OverlapBox(trigger.transform.position, triggerArea, Quaternion.identity, LayerMask.GetMask(LayerMask.LayerToName(layer)));
            
            for(int i = 0; i < cols.Length - 1; i++)
            {
                if (cols[i].gameObject == checkObject)
                    conditionIsMet = true;
                else
                    conditionIsMet = false;
            }

            yield return null;
        }
    }
}
