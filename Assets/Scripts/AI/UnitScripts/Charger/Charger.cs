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
			pathAgent.speed = 0;
			pathAgent.turnSpeed = 0;
			pathAgent.enabled = false;
			rb = GetComponent<Rigidbody>();
			StartCoroutine(Charge(player, waitTime));
			callOnce = true;
		}
		
	}

	IEnumerator Charge(GameObject target, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		isCharging = true;
		Vector3 chargeTarget = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
		transform.LookAt(chargeTarget);
		Vector3 dir = chargeTarget - transform.position;
		dir = dir.normalized;
		shieldEffect = Instantiate(chargeShield, shieldPoint.position, shieldPoint.rotation);
		shieldEffect.transform.parent = shieldPoint.transform;
		rb.AddForce(dir * force,ForceMode.Impulse);
	}

	//private void OnCollisionEnter(Collision collision)

	//{
	//	if (collision.gameObject.tag == "Player")
	//	{

	//		if (isCharging)
	//		{
	//			shieldEffect.GetComponent<ChargeShield>().Unping();
	//			Destroy(shieldEffect.gameObject);
	//			isCharging = false;
	//		}
	//	}
	//}
	public override void CollisionEvents()
	{
		//if (isCharging)
		//{
		//	shieldEffect.GetComponent<ChargeShield>().Unping();
		//	Destroy(shieldEffect.gameObject);
		//	isCharging = false;
		//}

	}
}
