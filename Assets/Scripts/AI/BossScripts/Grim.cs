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
	public float rotationSpeed;
	public float rotationSpeed2;
	public float phaseOneRange;
	public Transform phase2Place;
	[SerializeField]
	private BossAttacks[] attacks;
	[SerializeField]
	private GameObject[] phase1Colliders;
	[SerializeField]
	private GameObject[] phase2Colliders;
	[SerializeField]
	private BossLevelManager lvlMn;

	private BossPhases currentPhase;
	private GameObject currentPrefab;
	private Collider currentAttackTrigger;

	[HideInInspector]
	public bool isAttacking = false;
	private int gemCount=0;
	void Start() {
		currentPhase = new BossPhaseOne(this);
		ToggleHitbox(phase1Colliders, true);
	}




	public void IfAttacking()
	{
		isAttacking = !isAttacking;
		EndAttack();
	}


	public void StartAttack(int a)
	{
		EndAttack();
		if (attacks[a].AttackPrefab)
			currentPrefab = Instantiate(attacks[a].AttackPrefab, attacks[a].AttackPoints.position, attacks[a].AttackPoints.rotation);
		if (a == 4 || a ==6 )
		{
			currentPrefab.transform.parent = attacks[a].AttackPoints;
		}
		currentAttackTrigger = attacks[a].AttackTrigger;
		currentAttackTrigger.enabled = true;
	}

	public void EndAttack()
	{
		if (currentAttackTrigger)
		{
			currentAttackTrigger.enabled = false;
		}
		if (currentPrefab)
			Destroy(currentPrefab);
	}
	
	//public void CrystalHit()
	//{
	//	BulletType current = crystals[0].effectState;
	//	if (current == BulletType.Electric || current == BulletType.Ice)
	//	{
	//		for (int i = 0; i < crystals.Count; i++)
	//		{
	//			if (crystals[i].effectState != current)
	//				break;
	//			if (i == crystals.Count - 1)
	//			{
	//				Impair(current);
	//			}
	//		}
	//	}
	//}

	//private void Impair(BulletType a) {
	//	if (a == BulletType.Electric)
	//		currentPhase.Stun();
	//	else
	//		currentPhase.Slow();
	//}

	//private void ResetStun()
	//{
	//	currentPhase.UnStun();
	//}
	//private void ResetSlow()
	//{
	//	currentPhase.UnSlow();
	//}

	public void ChangeState(int a)
	{
		if (a == 1)
		{
			currentPhase = new BossPhaseTwo(this);
			ToggleHitbox(phase1Colliders, false);
			ToggleHitbox(phase2Colliders, true);
		}
	
	} 


	//public void freeze(int a)
	//{
	//	RaycastHit hit;
	//	Ray ray = new Ray(raypoint.transform.position, -transform.up);
	//	if (Physics.Raycast(ray, out hit))
	//	{
	//		if (hit.collider.isTrigger)
	//		{
	//			if (hit.collider.tag == "Freezable"&&!leftFrozen&&!rightFrozen)
	//			{
	//				currentPhase.TogglePause(true);
	//				if (a == 0)
	//				{
	//					GetComponent<Animator>().SetTrigger("RightFreeze");
	//					leftFrozen = true;
	//				}
	//				else
	//				{
	//					GetComponent<Animator>().SetTrigger("RightFreeze");
	//					rightFrozen = true;
	//				}
	//				StartCoroutine(Unfreeze());

	//			}
	//		}
	//	}
		
	//}

	//private IEnumerator Unfreeze()
	//{
	//	yield return new WaitForSeconds(fountainFreezeDuration);
	//	if(leftFrozen)
	//	leftFrozen = false;
	//	else 
	//	rightFrozen = false;
	//	GetComponent<Animator>().SetTrigger("Unfreeze");
	//	yield return new WaitForSeconds(2.8f);
	//	currentPhase.TogglePause(false);

	//}

	public void CrystalDestroy(int a) {
		if (a == 0)
		{
			GetComponent<Animator>().SetTrigger("Transition1");
			lvlMn.Transition();
			currentPhase.EndState();
		}
		else if (a == 1)
		{
			gemCount++;
			if (gemCount >= 2)
			{
				GetComponent<Animator>().SetTrigger("Die");

				currentPhase.EndState();
			}
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
