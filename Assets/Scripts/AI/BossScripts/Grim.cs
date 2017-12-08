﻿using System.Collections;
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
	public float rotationSpeed2;
	public float phaseOneRange;
	public float fountainFreezeDuration;
	public BossCrystals[] setCrystals;

	[SerializeField]
	private BossAttacks[] attacks;
	[SerializeField]
	private GameObject[] phase1Colliders;
	[SerializeField]
	private GameObject[] phase2Colliders;
	[SerializeField]
	private GameObject[] phase3Colliders;

	private BossPhases currentPhase;
	private GameObject currentPrefab;
	private Collider currentAttackTrigger;
	private bool leftFrozen;
	private bool rightFrozen;

	[HideInInspector]
	public bool isAttacking = false;
	[HideInInspector]
	public List<BossCrystals> crystals;

	void Start() {
		currentPhase = new BossPhaseOne(this);
		ToggleHitbox(phase1Colliders,true);
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
		{
			currentPhase = new BossPhaseTwo(this);
			ToggleHitbox(phase1Colliders,false);
			ToggleHitbox(phase2Colliders,true);
		}
		else
		{
			currentPhase = new BossPhaseThree(this);
			ToggleHitbox(phase2Colliders,false);
			ToggleHitbox(phase3Colliders,true);
		}
	} 


	public void freeze(int a)
	{
		RaycastHit hit;
		Ray ray = new Ray(raypoint.transform.position, -transform.up);
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.isTrigger)
			{
				if (hit.collider.tag == "Freezable"&&!leftFrozen&&!rightFrozen)
				{
					currentPhase.TogglePause(true);
					if (a == 0)
					{
						GetComponent<Animator>().SetTrigger("RightFreeze");
						leftFrozen = true;
					}
					else
					{
						GetComponent<Animator>().SetTrigger("RightFreeze");
						rightFrozen = true;
					}
					StartCoroutine(Unfreeze());

				}
			}
		}
		
	}

	private IEnumerator Unfreeze()
	{
		yield return new WaitForSeconds(fountainFreezeDuration);
		if(leftFrozen)
		leftFrozen = false;
		else 
		rightFrozen = false;
		GetComponent<Animator>().SetTrigger("Unfreeze");
		yield return new WaitForSeconds(2.8f);
		currentPhase.TogglePause(false);

	}

	public void CrystalDestroy(BossCrystals a) {

		if (crystals.Contains(a))
		{
			crystals.Remove(a);
			Destroy(a._crystal.gameObject);

		}
		if (crystals.Count <= 0)
		{
			GetComponent<Animator>().SetTrigger("Transition1");
			currentPhase.EndState();
		}

	}

	private void ToggleHitbox(GameObject[] c,bool a)
	{
		for (int i = 0; i < c.Length; i++)
		{
			c[i].SetActive(a);
		}
	}

}
