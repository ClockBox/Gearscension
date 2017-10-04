using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AreaCondition : Condition
{
    public GameObject checkObject;

    public Bounds triggerArea = new Bounds(Vector3.zero, Vector3.one * 2);
    private Collider[] cols;

    public override bool checkCondition()
    {
        if (checkObject)
        {
            cols = Physics.OverlapBox(gameObject.transform.position + triggerArea.center, triggerArea.extents, checkObject.transform.rotation, ~checkObject.gameObject.layer);
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
        Vector3 center = triggerArea.center + gameObject.transform.position;
        Transform rotation = gameObject.transform;

        float x = triggerArea.extents.x;
        float y = triggerArea.extents.y;
        float z = triggerArea.extents.z;

        Handles.color = new Color(1, 0, 0.5f, 0.5f);
        Handles.DrawLines(new Vector3[]
        {
            rotation.right * x + rotation.up * y + rotation.forward * z + center,
            rotation.right * -x + rotation.up * y + rotation.forward * z + center,
            rotation.right * x + rotation.up * y + rotation.forward * z + center,
            rotation.right * x + rotation.up *- y + rotation.forward * z + center,
            rotation.right * x + rotation.up * y + rotation.forward * z + center,
            rotation.right * x + rotation.up * y + rotation.forward * -z + center,
            rotation.right * -x + rotation.up * -y + rotation.forward * -z + center,
            rotation.right * x + rotation.up * -y + rotation.forward * -z + center,
            rotation.right * -x + rotation.up * -y + rotation.forward * -z + center,
            rotation.right * -x + rotation.up * y + rotation.forward * -z + center,
            rotation.right * -x + rotation.up * -y + rotation.forward * -z + center,
            rotation.right * -x + rotation.up * -y + rotation.forward * z + center,
            rotation.right * x + rotation.up * y + rotation.forward * -z + center,
            rotation.right * -x + rotation.up * y + rotation.forward * -z + center,
            rotation.right * x + rotation.up * y + rotation.forward * -z + center,
            rotation.right * x + rotation.up * -y + rotation.forward * -z + center,
            rotation.right * x + rotation.up * -y + rotation.forward * z + center,
            rotation.right * x + rotation.up * -y + rotation.forward * -z + center,
            rotation.right * x + rotation.up * -y + rotation.forward * z + center,
            rotation.right * -x + rotation.up * -y + rotation.forward * z + center,
            rotation.right * -x + rotation.up * y + rotation.forward * z + center,
            rotation.right * -x + rotation.up * -y + rotation.forward * z + center,
            rotation.right * -x + rotation.up * y + rotation.forward * z + center,
            rotation.right * -x + rotation.up * y + rotation.forward * -z + center,
        });
    }
}
