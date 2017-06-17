using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberAI : MonoBehaviour {
	GameObject player;
	Rigidbody rb;
	public bool activated;
	UnitPathFinding pathAgent;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody>();
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
					StartCoroutine(launchExplode(1f));
		
			}
		}
	}

	IEnumerator launchExplode(float delayTimer)
	{
		yield return new WaitForSeconds(delayTimer);


		
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

    private void OnCollisionEnter(Collision collision)
    {
        explode();
    }
    void explode()
    {


    }
}
