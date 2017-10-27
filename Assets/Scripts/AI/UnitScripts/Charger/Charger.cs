using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : AIStateManager {
	public float waitTime;
	public Transform shieldPoint;
	public GameObject chargeShield;
	private GameObject shieldEffect;
	public float force;
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
			pathAgent.speed = 0;
			pathAgent.turnSpeed = 0;
			pathAgent.enabled = false;
			
			transform.LookAt(player.transform);
			shieldEffect = Instantiate(chargeShield, shieldPoint.position, shieldPoint.rotation);
			shieldEffect.transform.parent = this.transform;
			StartCoroutine(Charge(player.transform.position,waitTime));
			callOnce = true;
		}
		
	}

	IEnumerator Charge(Vector3 target, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		Vector3 dir = (target - transform.position).normalized;
		GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);

	}

	public override void CollisionEvents()
	{
		

	}
}
