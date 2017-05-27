using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gregAttacks : MonoBehaviour {

	public Collider[] attackHitBoxes;
	Animator anim;
	public GameObject magnet;
	public Transform magnetPos;
	public GameObject explosive;
	public Transform explosivePos;
	public GameObject killFloor;
	public Transform killFloorPos;
	GameObject player;
	
	void Start () {
		anim = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void chooseAttack1()
	{
		if (Vector3.Distance(attackHitBoxes[0].transform.position, player.transform.position) < Vector3.Distance(attackHitBoxes[1].transform.position, player.transform.position))
		{
			lStomp();
		}
		else {
			rStomp();
			int num = Random.Range(0, 2);
			if (num == 0)
			{
				Invoke("lStomp", 3f);
			}

		}
	}

	public void chooseAttack2()
	{
	}
	public void chooseAttack3()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.G)){
			lStomp();

		}
		if (Input.GetKeyDown(KeyCode.H))
		{
			rStomp();

		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			hSlam();

		}
		if (Input.GetKeyDown(KeyCode.J))
		{
			sweep();

		}




	}



	void lStomp()
	{
		anim.SetTrigger("lStomp");
		LaunchAttack(attackHitBoxes[0]);

	}
	void rStomp()
	{

		anim.SetTrigger("rStomp");
		LaunchAttack(attackHitBoxes[1]);

	}
	void hSlam()
	{

		anim.SetTrigger("Slam");
		LaunchAttack(attackHitBoxes[3]);
		LaunchAttack(attackHitBoxes[4]);
	}
	void sweep()
	{

		anim.SetTrigger("Sweep");
		LaunchAttack(attackHitBoxes[2]);

	}


	private void LaunchAttack(Collider c)
	{
		Collider[] cols = Physics.OverlapBox(c.bounds.center, c.bounds.extents, c.transform.rotation, LayerMask.GetMask("Hitbox"));
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
					damage = 3;
					break;
				case "leftFootHitBox":
					damage = 3;
					break;
				case "rightFootHitBox":
					damage = 3;
					break;
				case "lHandHitBox":
					damage = 3;
					break;
				case "rHandHitBox":
					damage = 3;
					break;
				default:
					Debug.Log("Unable to identify hitbox name");
					break;

			}
			col.SendMessageUpwards("TakeDamage", damage);
		}
	}
	void rightFootStomp()
	{
		if (magnet)
		{
			Instantiate(magnet, magnetPos.position, magnetPos.rotation);
		}
	}
	void leftFootStomp()
	{
		if (explosive)
		{
			Instantiate(explosive, explosivePos.position, explosivePos.rotation);

		}
	}

	void handSlam()
	{
		if (killFloor)
		{
			Instantiate(killFloor, killFloorPos.position, killFloorPos.rotation);

		}
	}
}
