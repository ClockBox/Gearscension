using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : ElectricalObject
{
    protected void OnTriggerStay(Collider col)
    {
        GameObject temp = col.attachedRigidbody.gameObject;
        if(temp && (temp.CompareTag("Player") || temp.CompareTag("Freezable")))
            col.attachedRigidbody.gameObject.transform.parent = GetComponentInChildren<Transform>();
    }
    protected void OnTriggerExit(Collider col)
    {
        GameObject temp = col.attachedRigidbody.gameObject;
        if (temp && (temp.CompareTag("Player") || temp.CompareTag("Freezable")))
        {
            col.attachedRigidbody.gameObject.transform.parent = null;
            DontDestroyOnLoad(col.gameObject);
        }
    }
}
