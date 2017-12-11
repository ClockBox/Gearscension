using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireball : MonoBehaviour {
	[SerializeField]
	private GameObject fireball;

	[SerializeField]
	private int force;
	[SerializeField] private int radius;
	private void Awake()
	{
		int i = Random.Range(6, 15);
		for (int a = 0;a< i; a++) {
			float x = Random.Range(transform.position.x - 2, transform.position.x + 2);
			float y = Random.Range(transform.position.y+2, transform.position.y + 4);
			float z = Random.Range(transform.position.z-2, transform.position.z + 2);
			GameObject rb= Instantiate(fireball, new Vector3(x,y,z), transform.rotation);
			rb.GetComponentInChildren<Rigidbody>().AddExplosionForce(force, transform.position, radius);
         
        }
        Destroy(this.gameObject, 1f);
	} 

}
