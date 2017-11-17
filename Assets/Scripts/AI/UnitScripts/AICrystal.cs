using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICrystal : MonoBehaviour {
	private bool destroyed = false;
	public void TakeDamage(float damage)
	{
		if (damage > 0 && !destroyed)
		{
			Debug.Log("crystal damaged");
			gameObject.GetComponentInParent <AIStateManager>().Die();
			destroyed = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Sword"))
		{
			
			TakeDamage(5f);
		}
	}
}
