using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testp : MonoBehaviour {
	public float health;
	public void TakeDamage(float damage)
	{
		health -= damage;
	}
}
