using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPusher : MonoBehaviour {

	public float pushRadius = 3f;
	public float finishTime;
	public float lifeTime = 3f;
	// Use this for initialization
	void Start()
	{
		finishTime = 100f;
	}


	void Update()
	{
		lifeTime -= Time.deltaTime;
		if (lifeTime <= 0)
		{
			Destroy(this.gameObject);
		}
		foreach (Collider collider in Physics.OverlapSphere(transform.position, pushRadius, LayerMask.GetMask("Hitbox")))
		{
			Vector3 direction = collider.GetComponent<Transform>().position - transform.position;
			float distance = Vector3.Distance(transform.position, collider.GetComponent<Transform>().position);
			Vector3 target = collider.GetComponent<Transform>().position + direction * (2.5f - distance);
			collider.GetComponent<Transform>().position = Vector3.Lerp(collider.GetComponent<Transform>().position, target, lifeTime / finishTime);
		}
	}



}
