using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class AreaCondition : Condition
{
    public GameObject checkObject;
    public bool UseColider;

    public Bounds triggerArea = new Bounds(Vector3.zero, Vector3.one * 2);
    protected Collider[] cols;

    private Vector3 center;

    public override bool CheckCondition()
    {
        center = transform.position +
            transform.right * triggerArea.center.x +
            transform.up * triggerArea.center.y +
            transform.forward * triggerArea.center.z;

        if (checkObject)
        {
            cols = Physics.OverlapBox(center, triggerArea.extents, transform.rotation, ~checkObject.gameObject.layer);
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
        center = transform.position +
               transform.right * triggerArea.center.x +
               transform.up * triggerArea.center.y +
               transform.forward * triggerArea.center.z;

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
