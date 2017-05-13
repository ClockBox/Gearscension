using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBreakable : MonoBehaviour {
	public bool broken = false;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update()
	{
		if (broken)
		{
			transform.parent = null;
			gameObject.AddComponent<Rigidbody>();
		}
	}

}
