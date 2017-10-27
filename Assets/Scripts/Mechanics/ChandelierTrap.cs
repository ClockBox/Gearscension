using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierTrap : MonoBehaviour
{
    public GameObject[] breakablePart;
    private Rigidbody rb;

    private void Start()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Boss")
        {
            for (int i = 0; i < breakablePart.Length; i++)
            {
                breakablePart[i].GetComponent<Rigidbody>().useGravity = true;
                breakablePart[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                breakablePart[i].transform.parent = null;
                breakablePart[i].GetComponent<Collider>().enabled = true;
            }
        }
    }
}
