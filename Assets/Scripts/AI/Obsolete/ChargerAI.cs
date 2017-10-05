using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAI : MonoBehaviour {
	GameObject player;
	public bool activated;
	public float chargeCD = 5;
	float chargeTimer;
	Rigidbody rb;
	UnitPathFinding pathAgent;

	float force = 50;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		rb = gameObject.GetComponent<Rigidbody>();
		chargeTimer = chargeCD;
		pathAgent = GetComponent<UnitPathFinding>();

	}

	// Update is called once per frame
	void Update () {
		if (activated)
		{
			if (Vector3.Distance(player.transform.position, transform.position) >= 100f)
			{
				pathAgent.travel(player.transform.position);
			}
			else
			{


				if (chargeTimer >= chargeCD)
				{
					StartCoroutine(Charge(player, 1f));
				}
				else
					chargeTimer += Time.deltaTime;
			}
		}

		
	}

	IEnumerator Charge(GameObject target,float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		transform.LookAt(target.transform);
		Vector3 dir = target.transform.position - transform.position;
		dir = dir.normalized;
		rb.AddForce(dir * force);
	}

	private void OnCollisionEnter(Collision collision)
	{
		stopCharge();
	}
	void stopCharge()
	{
		chargeTimer = 0;
	}

}
