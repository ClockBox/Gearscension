using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
    [SerializeField]
    private string checkTag;
    [SerializeField]
    private GameObject[] breakablePart;

    private Rigidbody temp;
    private MeshCollider meshCol;


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == checkTag)
        {
            for (int i = 0; i < breakablePart.Length; i++)
            {
                if (breakablePart[i].GetComponent<Rigidbody>() == null)
                    breakablePart[i].AddComponent<Rigidbody>();
                if (breakablePart[i].GetComponent<BoxCollider>() == null)
                    breakablePart[i].AddComponent<BoxCollider>();
                breakablePart[i].layer = LayerMask.NameToLayer("Debris");
                breakablePart[i].transform.parent = null;
                Destroy(breakablePart[i].gameObject, 5);
            }
        }
        GetComponent<Collider>().enabled = false;
    }
}
