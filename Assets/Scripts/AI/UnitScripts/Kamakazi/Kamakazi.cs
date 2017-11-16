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
	private Vector3 playerPos;
	private bool heightReached;
	private GameObject smoker;

	public override void StartEvents() {

		smoker = Instantiate(smokePrefab, exhaust.transform.position, transform.rotation);
		smoker.transform.parent = transform;
	}


	public override void MeleeAttack()
	{
	}

	public override void RangedAttack()
	{
		if (!callOnce)
		{
			pathAgent.angularSpeed = 0;
			pathAgent.enabled = false;
			rb = GetComponent<Rigidbody>();
			callOnce = true;
			exploded = false;
			heightReached = false;
			smoker.GetComponent<ParticleSystem>().Play();

		}
		StartCoroutine(LaunchExplode(2f));

	}
	IEnumerator LaunchExplode(float delayTimer)
	{
		yield return new WaitForSeconds(delayTimer);



		rb.constraints = RigidbodyConstraints.FreezeRotation;
		if (transform.position.y <= maxHeight && !heightReached)
		{
			rb.AddForce(transform.up * force/10, ForceMode.Impulse);

		}
		else if (transform.position.y > maxHeight)
		{
			playerPos = player.transform.position;
			playerPos = new Vector3(playerPos.x, playerPos.y - 2, playerPos.z);
			smoker.GetComponent<ParticleSystem>().Play();

			heightReached = true;
		}

		if(heightReached)

		{
			
			Vector3 target = playerPos - transform.position;
			target.Normalize();
			rb.AddForce(target *force, ForceMode.Impulse);
			//rb.AddForce(transform.up * -force, ForceMode.Impulse);

		}

		//if (Vector3.Distance(transform.position, player.transform.position) <= proximity)
		//{
		//	if(!exploded)
		//	Explode();
		//}

	}

	private void OnCollisionEnter(Collision collision)

	{

		if (callOnce)
		{
			Explode().transform.parent = collision.transform;
			Die();

		}
	} 

	GameObject Explode()
	{
		GameObject newEffect = Instantiate(effectPrefab, transform.position, transform.rotation);
		newEffect.transform.localScale = transform.localScale;
		exploded = true;
		return newEffect;
		
	}
	

}
