using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCondition : Condition
{
    Collider[] col;
    [SerializeField]
    protected int layer;

    Vector3 triggerCol;

    public AreaCondition(Trigger trigger, string name, GameObject player) : base(trigger, name, player)
    {
        triggerCol = trigger.GetComponent<Collider>().bounds.extents;
        trigger.StartCoroutine(CheckArea());
    }

    IEnumerator CheckArea()
    {
        while (true)
        {
            col = Physics.OverlapBox(trigger.transform.position, triggerCol, Quaternion.identity, LayerMask.GetMask(LayerMask.LayerToName(layer)));
            
            for(int i = 0; i < col.Length - 1; i++)
            {
                if (col[i].gameObject == (GameObject)checkObject)
                    conditionIsMet = true;
                else
                    conditionIsMet = false;
            }

            yield return null;
        }
    }
}
