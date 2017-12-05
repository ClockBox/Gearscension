using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;

public abstract class BossPhases {
	protected Animator anim;
	protected NavMeshAgent nmAgent;
	protected GameObject player;
	protected bool paused;
	public bool isActive;
	public float phaseRange;
	
	public BossPhases(Grim boss) {
		paused = false;
		isActive = true;
		anim = boss.GetComponent<Animator>();
		nmAgent = boss.GetComponentInParent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag("Player");
		Debug.Log("New Bossphase constructor called");
		Initialize(boss);
		boss.StartCoroutine(StartUpdate(boss));
	}

	public void TogglePause()
	{
		nmAgent.isStopped = !nmAgent.isStopped;
		//if (anim.speed > 0)
		//	anim.speed = 0;
		//else
		//	anim.speed = 1;
		paused = !paused;
	}
	public void EndState(Grim boss)
	{
		isActive = false;
	}

	public IEnumerator StartUpdate(Grim boss) {
		while (isActive)
		{
			if (!paused && !boss.isAttacking)
			{
				yield return boss.StartCoroutine(UpdateAction(boss));
			}
			yield return null;

		}
		Debug.Log("Phase ended");
	}

	public abstract void Initialize(Grim boss);
	public abstract IEnumerator UpdateAction(Grim boss);

}

public class BossPhaseOne : BossPhases
{
	public BossPhaseOne(Grim boss) : base(boss)
	{
	}

	public override void Initialize(Grim boss)
	{
		phaseRange = boss.phaseOneRange;
		Debug.Log("PhaseOne initialized");
		anim.SetTrigger("StartP1");		
	}

	public override IEnumerator UpdateAction(Grim boss)
	{
		if (Vector3.Distance(boss.transform.position, player.transform.position) > phaseRange)
		{
			nmAgent.SetDestination(player.transform.position);
			yield break;
		}
		else
		{
			boss.isAttacking = true;
			int num = UnityEngine.Random.Range(0, 2);
			if (num == 0)
			{
				anim.SetTrigger("P1LAttack");
				yield break;
			}
			else
			{
				yield return boss.StartCoroutine(P1Combo(boss));
			}
		}
	}


	IEnumerator P1Combo(Grim boss) {
		anim.SetTrigger("P1RAttack");
		while (boss.isAttacking)
		{
			yield return null;
		}
		if (Vector3.Distance(boss.transform.position, player.transform.position) < phaseRange)
		{
			boss.isAttacking = true;
			anim.SetTrigger("P1LAttack");
		}
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

	public override IEnumerator UpdateAction(Grim boss)
	{
		yield return null;
	}

}

public class BossPhaseThree : BossPhases
{
	public BossPhaseThree(Grim boss) : base(boss)
	{
	}

	public override void Initialize(Grim boss)
	{
		Debug.Log("PhaseThree initialized");

	}
	public override IEnumerator UpdateAction(Grim boss)
	{
		yield return null;
	}


}
