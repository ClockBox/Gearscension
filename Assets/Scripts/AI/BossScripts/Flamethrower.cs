using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour {

	

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player"){
			other.gameObject.SendMessage("TakeDamage", 5);
				}
	} 
}
