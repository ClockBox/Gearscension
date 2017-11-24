using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public abstract class BossPhases {
	protected Animator anim;
	protected NavMeshAgent nmAgent;
	protected GameObject player;
	public BossPhases(Grim boss) {
		anim = boss.GetComponent<Animator>();
		nmAgent = boss.GetComponentInParent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag("Player");
		Initialize(boss);
	}

	public abstract void Initialize(Grim boss);
	public abstract void HandleUpdate(Grim boss);
}

public class BossPhaseOne : BossPhases
{
	public BossPhaseOne(Grim boss) : base(boss)
	{

	}

	public override void Initialize(Grim boss)
	{

		Debug.Log("PhaseOne initialized");
		anim.SetTrigger("StartP1");
	}

	public override void HandleUpdate(Grim boss)
	{
		PhaseOne(boss);
	}

	private void PhaseOne(Grim boss)
	{
		nmAgent.SetDestination(player.transform.position);

		if (Vector3.Distance(boss.transform.position, player.transform.position) <= 5f)
		{
			nmAgent.isStopped = true;
			if(!boss.isAttacking)
			boss.StartCoroutine(AttackSequencing());

		}
		else
		{
			nmAgent.isStopped = false;
		}
	}
	IEnumerator AttackSequencing()
	{
		int num = Random.Range(0, 2);
		if (num == 0)
		{
			anim.SetTrigger("P1LAttack");
		}
		else
		{
			anim.SetTrigger("P1RAttack");
		}
	
		yield return new WaitForSeconds(2f);
		Debug.Log(player.transform.position+""+nmAgent.speed);
	}


}

public class BossPhaseTwo : BossPhases
{
	public BossPhaseTwo(Grim boss) : base(boss)
	{
	}

	public override void Initialize(Grim boss)
	{
		Debug.Log("PhaseTwo initialized");
	}

	public override void HandleUpdate(Grim boss)
	{
		PhaseTwo(boss);
	}

	private void PhaseTwo(Grim boss)
	{
	}
}

public class BossPhaseThree : BossPhases
{
	public BossPhaseThree(Grim boss) : base(boss)
	{
	}

	public override void Initialize(Grim boss)
	{
	}
	public override void HandleUpdate(Grim boss)
	{
		PhaseThree(boss);	
	}
	private void PhaseThree(Grim boss)
	{
	}
}
