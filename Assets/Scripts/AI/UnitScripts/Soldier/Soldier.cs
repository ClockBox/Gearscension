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
	private int choice;
	private float bulletRotation;
	
	public override void RangedAttack()
	{
		if (!callOnce)
		{
			shotFrequency = shotInterval-0.9f;
			GameObject smoke = Instantiate(smokePrefab, exhaust.transform.position, exhaust.transform.rotation);
			smoke.GetComponent<ParticleSystem>().Emit(1);
			Destroy(smoke, 1f);
			bulletRotation = -10;
			callOnce = true;
		}
		
		if (shotFrequency >= shotInterval)
		{
			Rigidbody _bullet;
			_bullet = Instantiate(bulletPrefab, gunPoint.transform.position, gunPoint.transform.rotation);
			_bullet.transform.Rotate(0, bulletRotation, 0);
			bulletRotation += 5;
			_bullet.velocity = _bullet.transform.forward* bulletSpeed;
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
			pathAgent.travel(player.transform.position);
			if (Vector3.Distance(transform.position, player.transform.position) <= meleeRange)
			{
				fireBurster.GetComponent<ParticleSystem>().Emit(2);
				player.gameObject.SendMessage("TakeDamage", meleeDamage, SendMessageOptions.DontRequireReceiver);
			}
			attackFrequency = 0;
		}
		attackFrequency += Time.deltaTime;

	}

	public override void Die()
	{
		if (isAlive)
		{
			Debug.Log("Soldier dead");

			Rigidbody[] temp = GetComponentsInChildren<Rigidbody>();
			if (temp.Length > 0)
			{
				for (int i = 0; i < temp.Length; i++)
				{
					temp[i].useGravity = true;
					temp[i].constraints = RigidbodyConstraints.None;
					temp[i].transform.parent = null;
					temp[i].GetComponent<BoxCollider>().enabled = true;

				}
			}
			
			Destroy(gameObject, 1f);
			isAlive = false;
		}
	}

	public override void StartEvents() {
		fireBurster = Instantiate(firePrefab, transform.position, transform.rotation);
		fireBurster.transform.parent = transform;
		
	}
	public override void CollisionEvents()
	{
		
	}

}
