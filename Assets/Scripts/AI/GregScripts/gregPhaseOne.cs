using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gregPhaseOne : MonoBehaviour {
	
	public float attackFrequency = 4f;

	public float speed = 1f;
	//Animator anim;
    public GameObject breakable;

    GameObject player;
    gregAttacks attackScript;
    private float attackCD;
    bool p1Alive = true;
    private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		attackScript = GetComponent<gregAttacks>();
        //anim = GetComponent<Animator>();
        attackCD = attackFrequency;
	}
	void Update()
	{
        if (p1Alive)
        {
            attackCD += Time.deltaTime;
            Walk();
            if (attackCD >= attackFrequency)
            {
                if (Vector3.Distance(transform.position, player.transform.position) < 15f)
                    Attack();
            }

        }
		//if (attackCD >= 4f&&anim.GetCurrentAnimatorStateInfo(0).IsName("walking_inPlace"))
		//{
		//	attackScript.chooseAttack1();

		//	attackCD = 0f;
		//}
		//else
		//{
		//	attackCD += Time.deltaTime;
		//}
        //if (!anim.GetBool("frozen"))
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        //    Vector3 point = player.transform.position;
        //    point.y = transform.position.y;

        //    transform.LookAt(point);

        //}

        //if (GetComponent<Rigidbody>().constraints == RigidbodyConstraints.FreezeAll)
        //{
        //    anim.SetBool("frozen", true);
        //    anim.enabled = false;
        //   }
        //else
        //{
        //    anim.enabled = true;
        //    anim.SetBool("frozen", false);
            
        //}
	}

   

    void Walk()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        Vector3 point = player.transform.position;
        point.y = transform.position.y;

        transform.LookAt(point);
    }

    void Attack()
    {
         attackScript.chooseAttack1();

         attackCD = 0f;

    }

   public void hitByPD()
    {
        Debug.Log("HITBYPD");
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //breakable.GetComponent<AIBreakable>().broken = true;
        GetComponent<Collider>().enabled = false;
        p1Alive = false;
    }

    public void CrystalDestroyed()
    {
        Debug.Log("BOOM");

    }


}
