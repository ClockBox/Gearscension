using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : AIStateManager {


	Rigidbody rb;
	public float force;
	public float waitTime;
	public Transform shieldPoint;
	public GameObject chargeShield;
	private bool isCharging=false;
	private GameObject shieldEffect;

	public override void StartEvents() { }

	public override void Die()
	{

	}

	public override void MeleeAttack()
	{

	}

	public override void RangedAttack()
	{
		if (!callOnce)
		{
			rb = GetComponent<Rigidbody>();
			StartCoroutine(Charge(player, waitTime));
			callOnce = true;
		}
		
	}

	IEnumerator Charge(GameObject target, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		isCharging = true;
		transform.LookAt(target.transform);
		Vector3 dir = target.transform.position - transform.position;
		dir = dir.normalized;
		shieldEffect = Instantiate(chargeShield, shieldPoint.position, shieldPoint.rotation);
		shieldEffect.transform.parent = gameObject.transform;
		rb.AddForce(dir * force,ForceMode.Impulse);
	}
	public override void CollisionEvents()
	{
		if (isCharging)
		{
			Destroy(shieldEffect.gameObject);
			isCharging = false;
		}

	}
}
