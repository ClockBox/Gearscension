using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeShield : MonoBehaviour {
	public float shieldTime;
	public float force;
	private bool hitPlayer=false;
	private GameObject target;
	private float returnMass;
	private void Start()
	{
		Destroy(this.gameObject, shieldTime);
		returnMass = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().mass;


	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			//Vector3 direction = (transform.position - other.transform.position).normalized;
			//direction.y = 0;
			//other.GetComponent<Rigidbody>().AddForce(force * direction, ForceMode.Impulse);
			hitPlayer = true;
			target = other.gameObject;
			target.GetComponent<Rigidbody>().mass = 0;
			
		}

		other.SendMessageUpwards("TakeDamage", 5, SendMessageOptions.DontRequireReceiver);



	}
	private void Update()
	{
		if (hitPlayer)
		{
			target.transform.position = transform.position;
			PlayerState.grounded = false;
		}
	
	}

	private void OnDestroy()
	{
		if(target)
		target.GetComponent<Rigidbody>().mass = returnMass;
		
	} 

}
