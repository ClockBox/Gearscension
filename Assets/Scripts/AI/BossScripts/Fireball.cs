using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.SendMessage("TakeDamage", 50f);
		}
		if (collision.gameObject.tag != "Boss") 
		Destroy(this.gameObject);
		
	} 
}
