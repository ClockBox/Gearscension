using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testp : MonoBehaviour {
	public GameObject target;
	public float dotrange;
	public float dotx;
	public float doty;
	public float dott;
	public float angle;

	private void Update()
	{
		dotrange = Vector3.Distance(transform.position,target.transform.position);
		dotx  = Vector3.Dot(transform.right, target.transform.position - transform.position);
		doty= Vector3.Dot(transform.up, target.transform.position - transform.position);
		dott = dotx * dotrange;
		 angle = Vector3.Angle(target.transform.position-transform.position, transform.forward);
	} 
}
