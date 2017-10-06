using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : AIStateManager {


	public Transform gunPoint;
	public Rigidbody bulletPrefab;
	public float bulletSpeed;
	public float shotInterval;
	public float attackInterval;
	public float rotateSpeed;
	private float shotFrequency;
	private float attackFrequency;
	public float meleeRange;
	public float meleeDamage;
	public float rotationMagnitude;
	private Quaternion rotationA;
	private Quaternion rotationB;
	private int choice;

	public override void RangedAttack()
	{
		if (!callOnce)
		{
			choice = UnityEngine.Random.Range(0, 2);
			rotationA = new Quaternion(transform.rotation.x, transform.rotation.y + rotationMagnitude, transform.rotation.z,transform.rotation.w);
			rotationB= new Quaternion(transform.rotation.x, transform.rotation.y - rotationMagnitude, transform.rotation.z, transform.rotation.w);
			shotFrequency = shotInterval;
			callOnce = true;
		}
		//transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
		if(choice==0)
			transform.rotation = Quaternion.Lerp(transform.rotation, rotationA, Time.deltaTime * rotateSpeed);
		else
			transform.rotation = Quaternion.Lerp(transform.rotation, rotationB, Time.deltaTime * rotateSpeed);

		if (shotFrequency >= shotInterval)
		{
			Rigidbody _bullet;
			_bullet = Instantiate(bulletPrefab, gunPoint.transform.position, gunPoint.transform.rotation);
			_bullet.velocity = transform.forward * bulletSpeed;
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
		}

		if (attackFrequency >= attackInterval)
		{
			pathAgent.travel(player.transform.position);
			if (Vector3.Distance(transform.position, player.transform.position) <= meleeRange)
			{
				player.gameObject.SendMessage("TakeDamage", meleeDamage, SendMessageOptions.DontRequireReceiver);
			}
			attackFrequency = 0;
		}
		attackFrequency += Time.deltaTime;

	}
	public override void AlertOthers()
	{
	}
	public override void Die()
	{
		Debug.Log("dead");
	}
}
