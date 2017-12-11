using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitbox : MonoBehaviour {

	[SerializeField]
	private Grim boss;

	[SerializeField]
	private string collisionTag;

	[SerializeField]
	private GameObject breakable;

	[SerializeField]
	private GameObject crystal;

	[SerializeField]
	private GameObject pileDriver;


	private void OnTriggerEnter(Collider other)
	{
		
		 if (collisionTag == other.gameObject.tag)
		{
			Debug.Log("Hit by " + collisionTag);
			boss.GetComponent<Animator>().SetTrigger("Damage");
			if (breakable)
			{
				Destroy(breakable.gameObject);
			}

			if (crystal)
			{
				Destroy(crystal);
			}
			if (pileDriver)
			{
				Destroy(pileDriver);
				boss.CrystalDestroy(0);
			}
			else
				boss.CrystalDestroy(1);
		}

	} 
}
