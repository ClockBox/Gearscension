using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {

    GameObject[] PileDrivers;
    public bool Activated = false;
    private void Start()
    {
        Activated = false;
        PileDrivers = GameObject.FindGameObjectsWithTag("PileDriver");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E)&&Activated==true)
            {
                GetClosest(PileDrivers).GetComponent<PileDriver>().Activate();
                Activated = false;
                Debug.Log("SHOOTDRIVER");
                Invoke("setActive", 1f);
            }
        }
    }

    GameObject GetClosest(GameObject[] pileDrivers)
    {
        GameObject cPDriver = null;
        float minDist = Mathf.Infinity;
        Vector3 Pos = transform.position;
        foreach (GameObject l in pileDrivers)
        {
            float dist = Vector3.Distance(l.transform.position, Pos);
            if (dist < minDist)
            {
                cPDriver = l;
                minDist = dist;
            }

        }
        return cPDriver;
    }

    void setActive()
    {
        Debug.Log("ACTIVE");
        Activated = true;
    }
}
