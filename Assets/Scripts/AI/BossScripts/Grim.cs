using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[System.Serializable ]
public class BossAttacks
{	
	[SerializeField]
	protected Transform attackPoints;
	public Transform AttackPoints
	{
		get { return attackPoints; }
	}

	[SerializeField]
	protected GameObject attackPrefab;
	public GameObject AttackPrefab
	{
		get { return attackPrefab; }
	}

	[SerializeField]
	protected Collider attackTrigger;
	public Collider AttackTrigger
	{
		get { return attackTrigger; }
	}

}

public class Grim : MonoBehaviour {
	public Transform raypoint;
	public float rotationSpeed;
	public float phaseOneRange;
	[HideInInspector]
	public bool isAttacking = false;
	public BossCrystals[] setCrystals;
	public List<BossCrystals> crystals;
	[SerializeField]
	private BossAttacks[] attacks;


	public float fountainFreezeDuration;
	private BossPhases currentPhase;
	private GameObject currentPrefab;
	private Collider currentAttackTrigger;

	public bool leftFrozen;
	public bool rightFrozen;

	void Start() {
		currentPhase = new BossPhaseOne(this);
	}




	public void IfAttacking()
	{
		isAttacking = !isAttacking;
		EndAttack();
	}


	public void StartAttack(int a)
	{
		EndAttack();
		//currentPrefab = Instantiate(attacks[a].AttackPrefab, attacks[a].AttackPoints.position, attacks[a].AttackPoints.rotation);
		currentAttackTrigger = attacks[a].AttackTrigger;
		currentAttackTrigger.enabled = true;
	}

	public void EndAttack()
	{
		if (currentAttackTrigger)
		{
			currentAttackTrigger.enabled = false;
		}
		//if (currentPrefab)
		//	Destroy(currentPrefab, delayTimer);
	}
	
	public void CrystalHit()
	{
		BulletType current = crystals[0].effectState;
		if (current == BulletType.Electric || current == BulletType.Ice)
		{
			for (int i = 0; i < crystals.Count; i++)
			{
				if (crystals[i].effectState != current)
					break;
				if (i == crystals.Count - 1)
				{
					Impair(current);
				}
			}
		}
	}

	private void Impair(BulletType a) {
		if (a == BulletType.Electric)
			currentPhase.Stun();
		else
			currentPhase.Slow();
	}

	private void ResetStun()
	{
		currentPhase.UnStun();
	}
	private void ResetSlow()
	{
		currentPhase.UnSlow();
	}

	public void ChangeState(int a)
	{
		if (a == 1)
			currentPhase = new BossPhaseTwo(this);

		else
			currentPhase = new BossPhaseThree(this);
	} 


	public void freeze(int a)
	{
		RaycastHit hit;
		Ray ray = new Ray(raypoint.transform.position, -transform.up);
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.isTrigger)
			{
				if (hit.collider.tag == "Water"&&!leftFrozen&&!rightFrozen)
				{
					currentPhase.TogglePause(true);
					Invoke("Unfreeze", fountainFreezeDuration);
					if (a == 0)
					{
						GetComponent<Animator>().SetTrigger("LeftFreeze");
						leftFrozen = true;
					}
					else
					{
						GetComponent<Animator>().SetTrigger("RightFreeze");
						rightFrozen = true;
					}
				}
			}
		}
		
	}

	private void Unfreeze()
	{
		leftFrozen = false;
		rightFrozen = false;
		currentPhase.TogglePause(false);
		GetComponent<Animator>().SetTrigger("Unfreeze");

	}

	public void CrystalDestroy(BossCrystals a) {

		if (crystals.Contains(a))
		{
			crystals.Remove(a);
			Destroy(a.gameObject);

		}
		if (crystals.Count <= 0)
		{
			GetComponent<Animator>().SetTrigger("Transition1");
			currentPhase.EndState();
		}

	}

}
