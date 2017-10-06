using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBullet : MonoBehaviour {

	public float damage;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{

			collision.gameObject.SendMessage("TakeDamage",damage, SendMessageOptions.DontRequireReceiver);
		}

	}
 
}
