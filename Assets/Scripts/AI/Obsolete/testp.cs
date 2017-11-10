using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testp : MonoBehaviour {

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Sword"))
			TakeDamage(2f);

	}
	public void TakeDamage(float damage)
	{
		Debug.Log("Enemy taking dmage!");

	}
}
