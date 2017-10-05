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
	private float shotFrequency=-1;
	private float attackFrequency = -1;
	public float meleeRange;
	public float meleeDamage;
	public override void RangedAttack()
	{
		if (shotFrequency < 0)
		{
			int i = Random.Range(0, 2);
			if (i == 0)
			{
				rotateSpeed = rotateSpeed * -1;
			}
			
			shotFrequency = shotInterval;
		}
		transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

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
		if (attackFrequency < 0)
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
}
