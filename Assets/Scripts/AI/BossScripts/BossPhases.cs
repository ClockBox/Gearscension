using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public abstract class BossPhases {
	public Animator anim;
	public NavMeshAgent nmAgent;
	public GameObject player;
	public BossPhases(Grim boss) {
		anim = boss.GetComponentInChildren<Animator>();
		nmAgent = boss.GetComponent<NavMeshAgent>();
		player = boss.player;
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
		nmAgent.SetDestination(player.transform.position);		

		if (Vector3.Distance(boss.transform.position, player.transform.position) <= 5f)
		{
			nmAgent.isStopped = true;
						
		}
		else
		{
			nmAgent.isStopped = false;
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

	public override void HandleUpdate(Grim boss)
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
	}
}
