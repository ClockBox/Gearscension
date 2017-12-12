using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour 
{
    [SerializeField]
    private GameObject fireRing;
    private void Update()
    {
        Vector3 direction = GetComponent<Rigidbody>().velocity.normalized;
        transform.LookAt(direction);
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag != "Boss")
		{

			if (collision.gameObject.CompareTag("Player"))
			{
				collision.gameObject.SendMessage("TakeDamage", 50f);
			}

			GameObject fr = Instantiate(fireRing, transform.position, new Quaternion(0, 0, 0, 0));
			Destroy(fr.gameObject, 2.1f);
			Destroy(this.gameObject);

		}
	}	 
}
