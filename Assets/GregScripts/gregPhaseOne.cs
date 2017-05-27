using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gregPhaseOne : MonoBehaviour {
	
	gregAttacks attackScript;
	public float attackCD = 4f;
	GameObject player;
	Vector3 point;
	public float speed = 1f;
	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		attackScript = GetComponent<gregAttacks>();

	}
	void Update()
	{
		

		if (attackCD >= 4f)
		{
			attackScript.chooseAttack1();

			attackCD = 0f;
		}
		else
		{
			attackCD += Time.deltaTime;
		}
		transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
		point = player.transform.position;
		point.y = transform.position.y;
		transform.LookAt(point);
	}

	

}
