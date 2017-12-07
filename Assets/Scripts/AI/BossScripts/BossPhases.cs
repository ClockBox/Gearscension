using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;

public abstract class BossPhases {
	protected Animator anim;
	protected NavMeshAgent nmAgent;
	protected GameObject player;
	protected Grim boss;
	protected bool paused;
	public bool isActive;
	public float phaseRange;
	
	public BossPhases(Grim _boss) {
		paused = false;
		isActive = true;
		boss = _boss;
		anim = boss.GetComponent<Animator>();
		nmAgent = boss.GetComponentInParent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag("Player");
		Debug.Log("New Bossphase constructor called");
		Initialize();
		boss.StartCoroutine(StartUpdate());
	}

	public void Stun()
	{
		if (anim.speed > 0)
		{
			TogglePause(true);
			anim.speed = 0;
			boss.Invoke("ResetStun", 3f);
		}
	
	}
	public void Slow()
	{
		if (anim.speed > 0.3f)
		{
			anim.speed = 0.3f;
			boss.Invoke("ResetSlow", 3f);

		}
	}

	public void UnStun()
	{
		TogglePause(false);
			if (anim.speed != 1)
			anim.speed = 1;
	}
	public void UnSlow()
	{
		if (anim.speed != 1)
			anim.speed = 1;
	}

	public void TogglePause(bool a)
	{
		if (a == true)
		{
			PauseFunctions(true);
			paused = true;
		}
		else
		{
			PauseFunctions(false);

			paused = false;
		}
	}
	public void EndState()
	{
		isActive = false;
	}

	public IEnumerator StartUpdate() {
		while (isActive)
		{
			if (!paused && !boss.isAttacking)
			{
				yield return boss.StartCoroutine(UpdateAction());
			}
			yield return null;

		}
		Debug.Log("Phase ended");
	}

	public abstract void Initialize();
	public abstract IEnumerator UpdateAction();
	public abstract void PauseFunctions(bool a);
}

public class BossPhaseOne : BossPhases
{
	public BossPhaseOne(Grim boss) : base(boss)
	{
	}

	public override void Initialize()
	{
		phaseRange = boss.phaseOneRange;
		boss.crystals = new List<BossCrystals>();
		boss.crystals.Add(boss.setCrystals[0]);
		boss.crystals.Add(boss.setCrystals[1]);
		Debug.Log("PhaseOne initialized");
	}

	public override IEnumerator UpdateAction()
	{
		nmAgent.SetDestination(player.transform.position);
		if (Vector3.Distance(boss.transform.position, player.transform.position) > phaseRange)
		{
			
			yield break;
		}
		else
		{
			boss.isAttacking = true;
			int num = UnityEngine.Random.Range(0, 2);
			if (num == 0)
			{
				anim.SetTrigger("LeftAttack");
				yield break;
			}
			else
			{
				yield return boss.StartCoroutine(P1Combo());
			}
		}
	}

	public override void PauseFunctions(bool a)
	{
		nmAgent.isStopped = a;
	}


	private IEnumerator P1Combo() {
		anim.SetTrigger("RightAttack");
		while (boss.isAttacking)
		{
			yield return null;
		}
		if (Vector3.Distance(boss.transform.position, player.transform.position) < phaseRange)
		{
			boss.isAttacking = true;
			anim.SetTrigger("LeftAttack");
		}
	}

	
}

public class BossPhaseTwo : BossPhases
{
	private float turnLimit;
	public BossPhaseTwo(Grim boss) : base(boss)
	{
	}

	public override void Initialize()
	{
		Debug.Log("PhaseTwo initialized");
		boss.crystals.Clear();
		boss.crystals.Add(boss.setCrystals[2]);
		boss.crystals.Add(boss.setCrystals[3]);
		nmAgent.isStopped = true;
		turnLimit = 0;
	}

	public override IEnumerator UpdateAction()
	{
		
		Quaternion wantedRotation =
			Quaternion.LookRotation
			(new Vector3(player.transform.position.x, boss.transform.position.y, player.transform.position.z)
				- boss.transform.position);
		//Check Angle
		if (Quaternion.Angle(boss.transform.rotation, wantedRotation) > 15)
		{
			yield return boss.StartCoroutine(Turn(wantedRotation));
		}
		else
		{
			yield return boss.StartCoroutine(ChooseAttack());
			
		}
	}

	public override void PauseFunctions(bool a)
	{
	}

	private IEnumerator ChooseAttack()
	{
		boss.isAttacking = true;
		float distance = Vector3.Distance(boss.transform.position, player.transform.position);
		//Check Distance
		if (distance > 30)
		{
			yield return boss.StartCoroutine(FlameThrower());
		}
		else
		{
			yield return boss.StartCoroutine(Swipe());
		}

	}
	private IEnumerator Swipe()
	{

		if (getDot() > 0)
		{
			anim.SetTrigger("RightAttack");

		}
		else
		{
			int num = UnityEngine.Random.Range(0, 2);
			if (num == 0)
				anim.SetTrigger("Sweep");
			else
				anim.SetTrigger("LeftAttack");
		}
		yield return null;
	}

	private IEnumerator FlameThrower()
	{
		if (getDot() > 0)
		{
			anim.SetTrigger("RightFlame");
		}
		else
		{
			anim.SetTrigger("LeftFlame");
		}
		yield return null;
	}

	private IEnumerator Turn(Quaternion target)
	{
		boss.isAttacking = true;

		// change turn limit timer. or min rotation to trigger follow up attack after turning
		while (turnLimit<5&&Quaternion.Angle(boss.transform.rotation,target)>15)
		{
			turnLimit += Time.deltaTime;
			boss.transform.rotation = Quaternion.RotateTowards(boss.transform.rotation, target, Time.deltaTime * boss.rotationSpeed);
			if (getDot() > 0)
			{
				anim.SetBool("TurnRight", true);
			}
			else
			{
				anim.SetBool("TurnLeft", true);
			}
			yield return null;
		}
		anim.SetBool("TurnLeft", false);
		anim.SetBool("TurnRight", false);

		turnLimit = 0;
		yield return boss.StartCoroutine(ChooseAttack());
	}

	private float getDot()
	{
		float dot = Vector3.Dot(boss.transform.right, player.transform.position - boss.transform.position);
		return dot;
	}
}

public class BossPhaseThree : BossPhases
{
	public BossPhaseThree(Grim boss) : base(boss)
	{
	}

	public override void Initialize()
	{
		Debug.Log("PhaseThree initialized");
		boss.crystals.Clear();

	}
	public override IEnumerator UpdateAction()
	{
		yield return null;
	}
	public override void PauseFunctions(bool a)
	{
	}

}
