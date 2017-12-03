using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierWeapon : MonoBehaviour
{
	public float damage;
    public bool destroyOnHit;
	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Player")
		{
            Debug.Log("Hit By Soldier Weapon", this);
			collision.gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            if(destroyOnHit)
			    Destroy(gameObject);

		}
		else if (collision.gameObject.tag != "Enemy") 
		{
            if (destroyOnHit)
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
