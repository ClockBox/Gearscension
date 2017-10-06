using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    protected void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.transform.parent = GetComponentInChildren<Transform>();
        }
    }
    protected void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.transform.parent = null;
        }
    }
}
