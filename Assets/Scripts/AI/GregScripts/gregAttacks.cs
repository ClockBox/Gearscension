using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gregAttacks : MonoBehaviour {
	public Collider[] attackHitBoxes;
	public GameObject magnet;
	public GameObject explosive;
	public GameObject killFloor;
	public Transform killFloorPos;


    public Transform rightFootMagnetPos;
    public Transform leftFootExplodePos;
    GameObject player;



    //Animator anim;
	
        /*
         Attack HitBoxes:

        1: Left Foot
        2: Right Foot
         */
	void Start () {
		//anim = GetComponent<Animator>();
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
				Invoke("lStomp", 1f);
			}

		}
	}

	public void chooseAttack2()
	{
		if (Vector3.Distance(transform.position, player.transform.position) < 2f)
		{
			
				lStomp();
		
				rStomp();
			

		}
		else
		{

			int num = Random.Range(0, 2);
			if (num == 0)
			{
				hSlam();
			}
			else
				sweep();
		}


	

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
		//anim.SetTrigger("lStomp");
		LaunchAttack(attackHitBoxes[0]);
        StartCoroutine(spawnExplode(leftFootExplodePos, 1f));
	}
	void rStomp()
	{

		//anim.SetTrigger("rStomp");
		LaunchAttack(attackHitBoxes[1]);
        StartCoroutine(spawnMagnet(rightFootMagnetPos, 1f));

    }
    void hSlam()
	{

		//anim.SetTrigger("Slam");
		LaunchAttack(attackHitBoxes[3]);
		LaunchAttack(attackHitBoxes[4]);
		int num = Random.Range(0, 2);
		if (num == 0)
		{
			Invoke("hSlam", 0.5f);
		}

	}
	void sweep()
	{

		//anim.SetTrigger("Sweep");
		LaunchAttack(attackHitBoxes[2]);

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
					damage =50;
					break;
				default:
					Debug.Log("Unable to identify hitbox name");
					break;

			}
			col.SendMessageUpwards("TakeDamage", damage);
		}
	}
	IEnumerator spawnMagnet(Transform t,float delayTime)
	{
        yield return new WaitForSeconds(delayTime);
		if (magnet)
		{
			Instantiate(magnet, t.position, t.rotation);
		}
	}
    IEnumerator spawnExplode(Transform t,float delayTime)
	{
        yield return new WaitForSeconds(delayTime);

        if (explosive)
		{
			Instantiate(explosive, t.position, t.rotation);

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
