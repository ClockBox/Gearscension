using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AreaCondition : Condition
{
    public GameObject checkObject;
    public bool useCollider = true;

    public Bounds triggerArea;
    private Collider[] cols;

    public AreaCondition() { }
    public override void InitCondition(Trigger trigger)
    {
        base.InitCondition(trigger);
        if (useCollider)
        {
            triggerArea.center = trigger.transform.position;
            triggerArea.extents = trigger.GetComponent<Collider>().bounds.extents;
        }
        trigger.StartCoroutine(CheckArea());
    }

    IEnumerator CheckArea()
    {
        while (true)
        {
            cols = Physics.OverlapBox(triggerArea.center, triggerArea.extents, Quaternion.identity, LayerMask.GetMask(LayerMask.LayerToName(checkObject.gameObject.layer)));
            
            for(int i = 0; i < cols.Length - 1; i++)
            {
                if (cols[i].gameObject == checkObject)
                {
                    Debug.Log("InArea");
                    ConditionMet = true;
                }
                else
                    conditionIsMet = false;
            }

            yield return null;
        }
    }

    public override void OnDrawGizmos()
    {
        if (!useCollider)
        {
            Handles.color = Color.magenta;
            float x = triggerArea.extents.x;
            float y = triggerArea.extents.y;
            float z = triggerArea.extents.z;
            Handles.DrawLines (new Vector3[] 
                {
                    new Vector3(x, y, z) + triggerArea.center, new Vector3(-x, y, z) + triggerArea.center,
                    new Vector3(x, y, z) + triggerArea.center, new Vector3(x, -y, z) + triggerArea.center,
                    new Vector3(x, y, z) + triggerArea.center, new Vector3(x, y, -z) + triggerArea.center,
                    new Vector3(-x, -y, -z) + triggerArea.center, new Vector3(x, -y, -z) + triggerArea.center,
                    new Vector3(-x, -y, -z) + triggerArea.center, new Vector3(-x, y, -z) + triggerArea.center,
                    new Vector3(-x, -y, -z) + triggerArea.center, new Vector3(-x, -y, z) + triggerArea.center,
                    new Vector3(x, y, -z) + triggerArea.center, new Vector3(-x, y, -z) + triggerArea.center,
                    new Vector3(x, y, -z) + triggerArea.center, new Vector3(x, -y, -z) + triggerArea.center,
                    new Vector3(x, -y, z) + triggerArea.center, new Vector3(x, -y, -z) + triggerArea.center,
                    new Vector3(x, -y, z) + triggerArea.center, new Vector3(-x, -y, z) + triggerArea.center,
                    new Vector3(-x, y, z) + triggerArea.center, new Vector3(-x, -y, z) + triggerArea.center,
                    new Vector3(-x, y, z) + triggerArea.center, new Vector3(-x, y, -z) + triggerArea.center
                });
        }
    }
}
