using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Grim : MonoBehaviour {
	[HideInInspector]
	public bool canAttack;

	[SerializeField]
	private float attackSpeed;
	[SerializeField]
	private Collider[] hitBoxes;
	[SerializeField]
	private float phaseOneRange;


	/*
	 
		 
		 */

	private BossPhases currentPhase;
	private NavMeshAgent nmAgent;
	private float setAttackSpeed;
	private Animator anim;
	public GameObject player;

	void Start () {
		canAttack = true;
		anim = gameObject.GetComponentInChildren<Animator>();

		if (!anim)
		{
			Debug.Log("Grim ANIMATOR NOT FOUND");
		}

		nmAgent = gameObject.GetComponent<NavMeshAgent>();
		{
			if (!nmAgent)
			{
				Debug.Log("Navmesh Agent Not FOUND");
			}
		}

		player = GameObject.FindGameObjectWithTag("Player");
		if (!player)
		{
			Debug.Log("Player NOT FOUND");
		}

		if (attackSpeed > 0)
		{
			setAttackSpeed = attackSpeed;
		}
		else
			Debug.Log("Grim attackspeed not set or set below zero");


		currentPhase = new BossPhaseOne(this);


	}

	void Update () {
		currentPhase.HandleUpdate(this);
	}


	private IEnumerator CloseAnimation(float animationLength, Collider coll)
	{
		LaunchAttack(coll);
		yield return new WaitForSeconds(animationLength);

	} 

	private void LaunchAttack(Collider c)
	{
		Collider[] cols = Physics.OverlapBox(c.bounds.center, c.bounds.extents, c.transform.rotation, LayerMask.GetMask("Hitbox", "Character"));
		Debug.Log(c.name);
		foreach (Collider col in cols)
		{
			if (col.transform.root == transform)
				continue;


			Debug.Log(col.name);

			float damage = 0;
			switch (c.name)
			{
				case "rLegHitBox":
					damage = 50;
					break;
				case "LeftFootHitbox":
					damage = 50;
					break;
				case "RightFootHitbox":
					damage = 50;
					break;
				case "lHandHitBox":
					damage = 50;
					break;
				case "rHandHitBox":
					damage = 50;
					break;
				default:
					Debug.Log("Unable to identify hitbox name");
					break;

			}
			col.SendMessageUpwards("TakeDamage", damage);
		}
	}

	
	
}
