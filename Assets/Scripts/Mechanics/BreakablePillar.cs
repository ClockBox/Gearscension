using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePillar : MonoBehaviour
{
    [SerializeField]
    private GameObject[] breakablePart;

    private Rigidbody temp;
    private MeshCollider meshCol;
    
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Boss")
        {
            for(int i = 0; i < breakablePart.Length; i++)
            {
                if(breakablePart[i].GetComponent<Rigidbody>() == null)
                    temp = breakablePart[i].AddComponent<Rigidbody>();

                meshCol = breakablePart[i].AddComponent<MeshCollider>();
                meshCol.convex = true;
                meshCol.inflateMesh = true;
                temp.constraints = RigidbodyConstraints.None;
                temp.useGravity = true;
                temp.mass = 3000.0f;

                breakablePart[i].transform.parent = null;
            }

            transform.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
