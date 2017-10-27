using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePillar : MonoBehaviour
{
    public GameObject[] breakablePart;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
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

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.tag);
        if(col.gameObject.tag == "Boss")
        {
            for(int i = 0; i < breakablePart.Length; i++)
            {
                breakablePart[i].GetComponent<Rigidbody>().useGravity = true;
                breakablePart[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                breakablePart[i].transform.parent = null;
                breakablePart[i].GetComponent<Collider>().enabled = true;
            }
        }
    }
}
