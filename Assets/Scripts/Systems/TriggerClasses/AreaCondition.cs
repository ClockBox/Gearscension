using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AreaCondition : Condition
{
    public GameObject checkObject;

    public Bounds triggerArea;
    private Collider[] cols;

    public AreaCondition() { }
    public override void InitCondition(Trigger trigger)
    {
        base.InitCondition(trigger);
        trigger.StartCoroutine(CheckArea());
    }

    IEnumerator CheckArea()
    {
        while (true)
        {
            if (checkObject)
            {
                cols = Physics.OverlapBox(gameObject.transform.position + triggerArea.center, triggerArea.extents, Quaternion.identity, checkObject.gameObject.layer);
                for (int i = 0; i < cols.Length - 1; i++)
                {
                    if (cols[i].gameObject == checkObject)
                    {
                        Debug.Log("InArea");
                        ConditionMet = true;
                    }
                    else conditionIsMet = false;
                }
            }
            yield return null;
        }
    }

    public override void OnDrawGizmos()
    {
        Debug.Log(gameObject);
        Vector3 center = triggerArea.center + gameObject.transform.position;

        float x = triggerArea.extents.x;
        float y = triggerArea.extents.y;
        float z = triggerArea.extents.z;

        Handles.color = Color.magenta;
        Handles.DrawLines(new Vector3[]
            {
                new Vector3(x, y, z) + center, new Vector3(-x, y, z) + center,
                new Vector3(x, y, z) + center, new Vector3(x, -y, z) + center,
                new Vector3(x, y, z) + center, new Vector3(x, y, -z) + center,
                new Vector3(-x, -y, -z) + center, new Vector3(x, -y, -z) + center,
                new Vector3(-x, -y, -z) + center, new Vector3(-x, y, -z) + center,
                new Vector3(-x, -y, -z) + center, new Vector3(-x, -y, z) + center,
                new Vector3(x, y, -z) + center, new Vector3(-x, y, -z) + center,
                new Vector3(x, y, -z) + center, new Vector3(x, -y, -z) + center,
                new Vector3(x, -y, z) + center, new Vector3(x, -y, -z) + center,
                new Vector3(x, -y, z) + center, new Vector3(-x, -y, z) + center,
                new Vector3(-x, y, z) + center, new Vector3(-x, -y, z) + center,
                new Vector3(-x, y, z) + center, new Vector3(-x, y, -z) + center
            });
    }
}
