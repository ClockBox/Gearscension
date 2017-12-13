using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierWeapon : MonoBehaviour
{
	public float damage;
    public bool destroyOnHit;
	private void OnTriggerEnter(Collider collision)
	{
        if (collision.isTrigger)
            return;

		if (collision.gameObject.tag == "Player")
		{
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
}
