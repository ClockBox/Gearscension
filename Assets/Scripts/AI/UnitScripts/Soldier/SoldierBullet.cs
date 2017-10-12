﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBullet : MonoBehaviour {

	public float damage;


	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Debug.Log("Hit By Soldier Bullet");
			collision.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
			Destroy(gameObject);
		}

	}


	//{
	//	if (other.gameObject.tag == "Player")
	//	{
	//		Debug.Log("Damage");
	//		other.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
	//	}

	//} 
	//private void OnTrigger(Collision collision)
	//{
	//	if (collision.gameObject.tag == "Player")
	//	{
	//		Debug.Log("Damage");
	//		collision.gameObject.SendMessage("TakeDamage",damage, SendMessageOptions.DontRequireReceiver);
	//	}

	//}

}
