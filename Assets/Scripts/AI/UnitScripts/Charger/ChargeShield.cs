using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeShield : MonoBehaviour {
	public float shieldTime;
	public float force;
	private Vector3 target;
	private void Start()
	{
		Destroy(this.gameObject, shieldTime);
		RaycastHit ray;
		Vector3 pos = new Vector3(transform.position.x + 2, transform.position.y + 2, transform.position.z + 2);
		if (Physics.Raycast(pos, transform.forward,out ray, 100))
		{
			target=ray.transform.position;
			Debug.Log(ray.collider.gameObject.name);
		}
	} 
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
			{
			Vector3 pinPos = new Vector3(target.x, other.gameObject.transform.position.y+2, target.z);
			Vector3 direction = pinPos - other.transform.position;
			other.GetComponent<Rigidbody>().AddForce(force*direction, ForceMode.Impulse );			

			}

		other.SendMessageUpwards("TakeDamage", 5, SendMessageOptions.DontRequireReceiver);



	}

	

}
