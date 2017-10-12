using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamakazi : AIStateManager {
	Rigidbody rb;
	public GameObject effectPrefab;
	public float maxHeight;
	public float force;
	public float proximity;
	private bool exploded;


	public override void Die()
	{
		Destroy(gameObject);
	}
	public override void StartEvents() { }


	public override void MeleeAttack()
	{
	}

	public override void RangedAttack()
	{
		if (!callOnce)
		{
			rb = GetComponent<Rigidbody>();
			callOnce = true;
			exploded = false;
			

		}
		StartCoroutine(LaunchExplode(1f));

	}
	IEnumerator LaunchExplode(float delayTimer)
	{
		yield return new WaitForSeconds(delayTimer);



		rb.constraints = RigidbodyConstraints.FreezeRotation;
		if (transform.position.y <=maxHeight )
		{
			rb.AddForce(transform.up * force, ForceMode.Impulse);

		}

		else
		{
			Vector3 target = player.transform.position - transform.position;
			target.Normalize();
			rb.AddForce(target *force, ForceMode.Impulse);
			rb.AddForce(transform.up * -force, ForceMode.Impulse);

		}

		if (Vector3.Distance(transform.position, player.transform.position) <= proximity)
		{
			if(!exploded)
			Explode();
		}

	}

	private void OnCollisionEnter(Collision collision)

	{

		if (callOnce)
		{
			Explode().transform.parent = collision.transform;
		}
	} 

	GameObject Explode()
	{
		GameObject newEffect = Instantiate(effectPrefab, transform.position, transform.rotation);
		newEffect.transform.localScale = transform.localScale;
		Invoke("Die", 0.5f);
		exploded = true;
		return newEffect;
		
	}
	public override void CollisionEvents()
	{
	}

}
