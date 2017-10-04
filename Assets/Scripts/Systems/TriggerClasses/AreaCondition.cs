using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AreaCondition : Condition
{
    public GameObject checkObject;

    public Bounds triggerArea = new Bounds(Vector3.zero, Vector3.one * 2);
    protected Collider[] cols;

    public override bool CheckCondition()
    {
        if (checkObject)
        {
            cols = Physics.OverlapBox(transform.position + triggerArea.center, triggerArea.extents, transform.rotation, ~checkObject.gameObject.layer);
            for (int i = 0; i < cols.Length - 1; i++)
            {
                if (cols[i].gameObject == checkObject)
                    ConditionMet = true;
                else conditionIsMet = false;
            }
        }
        return conditionIsMet;
    }

    public void OnDrawGizmosSelected()
    {
        Vector3 center = triggerArea.center + transform.position;

        float x = triggerArea.extents.x;
        float y = triggerArea.extents.y;
        float z = triggerArea.extents.z;

        Handles.color = new Color(1, 0, 0.5f, 0.5f);
        Handles.DrawLines(new Vector3[]
        {
            transform.right * x + transform.up * y + transform.forward * z + center,
            transform.right * -x + transform.up * y + transform.forward * z + center,
            transform.right * x + transform.up * y + transform.forward * z + center,
            transform.right * x + transform.up *- y + transform.forward * z + center,
            transform.right * x + transform.up * y + transform.forward * z + center,
            transform.right * x + transform.up * y + transform.forward * -z + center,
            transform.right * -x + transform.up * -y + transform.forward * -z + center,
            transform.right * x + transform.up * -y + transform.forward * -z + center,
            transform.right * -x + transform.up * -y + transform.forward * -z + center,
            transform.right * -x + transform.up * y + transform.forward * -z + center,
            transform.right * -x + transform.up * -y + transform.forward * -z + center,
            transform.right * -x + transform.up * -y + transform.forward * z + center,
            transform.right * x + transform.up * y + transform.forward * -z + center,
            transform.right * -x + transform.up * y + transform.forward * -z + center,
            transform.right * x + transform.up * y + transform.forward * -z + center,
            transform.right * x + transform.up * -y + transform.forward * -z + center,
            transform.right * x + transform.up * -y + transform.forward * z + center,
            transform.right * x + transform.up * -y + transform.forward * -z + center,
            transform.right * x + transform.up * -y + transform.forward * z + center,
            transform.right * -x + transform.up * -y + transform.forward * z + center,
            transform.right * -x + transform.up * y + transform.forward * z + center,
            transform.right * -x + transform.up * -y + transform.forward * z + center,
            transform.right * -x + transform.up * y + transform.forward * z + center,
            transform.right * -x + transform.up * y + transform.forward * -z + center,
        });
    }
}
