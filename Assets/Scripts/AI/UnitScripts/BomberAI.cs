using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberAI : MonoBehaviour {
	public float vibrationTimer = 3f;
	GameObject player;
	Rigidbody rb;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void launchExplode()
	{
		if (vibrationTimer > 0f)
		{
			vibrationTimer -= Time.deltaTime;
		}
		else
		{
			rb.constraints = RigidbodyConstraints.FreezeRotation;
			if (transform.position.y <= 8f)
			{
				rb.AddForce(transform.up * 1.2f, ForceMode.Impulse);

			}

			else
			{
				Vector3 target = player.transform.position - transform.position;
				target.Normalize();
				rb.AddForce(target * 1.2f, ForceMode.Impulse);
				rb.AddForce(transform.up * -1.2f, ForceMode.Impulse);

			}
		}

	}
}
