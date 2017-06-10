using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gregPhaseTwo : MonoBehaviour {

	gregAttacks attackScript;
	public float attackCD = 4f;
	GameObject player;
	Vector3 point;
	public float speed = 1f;
	Animator anim;
	private void Start()
	{

		player = GameObject.FindGameObjectWithTag("Player");
		attackScript = GetComponent<gregAttacks>();
		anim = GetComponent<Animator>();
		anim.SetBool("phase2", true);

	}
	void Update()
	{


		if (attackCD >= 4f && anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
		{
			attackScript.chooseAttack2();

			attackCD = 0f;
		}
		else
		{
			attackCD += Time.deltaTime;
		}
		point = player.transform.position;
		point.y = transform.position.y;
		transform.LookAt(point);
	}
}
