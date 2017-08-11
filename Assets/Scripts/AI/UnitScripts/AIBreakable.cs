using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBreakable : MonoBehaviour {
	public bool broken = false;
    public GameObject crystal;
	// Use this for initialization
	void Start () {
        if (crystal==null)
        {
            Debug.Log("corresponding crystal not set");

        }
		
	}

	// Update is called once per frame
	void Update()
	{
		if (broken)
		{
            GetComponent<Rigidbody>().useGravity = true;
            crystal.GetComponent<AiCrystal>().exposed = true;
			transform.parent = null;
		}
	}

}
