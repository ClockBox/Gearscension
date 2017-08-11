using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {
    GameObject[] levers;
    private void Start()
    {
        levers = GameObject.FindGameObjectsWithTag("Levers");
    }
 
	public void generate()
    {
        GetClosest(levers).GetComponent<Lever>().Activated = true;

    }

    GameObject GetClosest(GameObject[] levers)
    {
        GameObject cLever = null;
        float minDist = Mathf.Infinity;
        Vector3 Pos = transform.position;
        foreach(GameObject l in levers)
        {
            float dist = Vector3.Distance(l.transform.position, Pos);
            if (dist < minDist)
            {
                cLever = l;
                minDist = dist;
            }
            
        }
        return cLever;
    }
}
