using System.Collections.Generic;
using UnityEngine;

public class Soldier : AIStateManager {


	public Transform gunPoint;
	public Rigidbody bulletPrefab;
	public float bulletSpeed;
	public float shotInterval;
	public float attackInterval;
	private float shotFrequency;
	private float attackFrequency;
	public float meleeRange;
	public float meleeDamage;
	private GameObject fireBurster;
	private Animator anim;
	public float predictionMagnitude;
	private Vector3 lastPlayerPosition;
	public override void RangedAttack()
	{
		if (!callOnce)
		{
			shotFrequency = shotInterval-0.9f;
			GameObject smoke = Instantiate(smokePrefab, exhaust.transform.position, exhaust.transform.rotation);
			smoke.GetComponent<ParticleSystem>().Emit(1);
			Destroy(smoke, 1f);
			lastPlayerPosition = player.transform.position;
			callOnce = true;

		}
		
		if (shotFrequency >= shotInterval)
		{
			Rigidbody _bullet;
			_bullet = Instantiate(bulletPrefab, gunPoint.transform.position, gunPoint.transform.rotation);
			Vector3 bulletDirection = (PredictPosition(lastPlayerPosition, player.transform.position) - _bullet.transform.position).normalized;
			_bullet.velocity =bulletDirection* bulletSpeed;
			Destroy(_bullet.gameObject, 5);
			shotFrequency = 0;
		}
		shotFrequency += Time.deltaTime;



	}
	public override void MeleeAttack()
	{
		if (!callOnce)
		{
			attackFrequency = attackInterval;
			callOnce = true;

		}

		if (attackFrequency >= attackInterval)
		{
			if (Vector3.Distance(transform.position, player.transform.position)>5f) 
				{
					pathAgent.speed = 8;
				}

				pathAgent.destination=player.transform.position;
			if (Vector3.Distance(transform.position, player.transform.position) <= meleeRange)
			{
				anim.SetTrigger("Attack");
				fireBurster.GetComponent<ParticleSystem>().Emit(2);
				player.gameObject.SendMessage("TakeDamage", meleeDamage, SendMessageOptions.DontRequireReceiver);
			}
			attackFrequency = 0;
		}
		attackFrequency += Time.deltaTime;

	}

	

	public override void StartEvents() {
		
		fireBurster = Instantiate(firePrefab, transform.position, transform.rotation);
		fireBurster.transform.parent = transform;
		anim = GetComponent<Animator>();
		
	}

	private Vector3 PredictPosition(Vector3 lastPos, Vector3 currentPos)
	{
		Vector3 target = (currentPos + (currentPos - lastPos).normalized * predictionMagnitude);
		return target;
	}
	

}
