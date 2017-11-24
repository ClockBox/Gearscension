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

}

public class Grim : MonoBehaviour {

	[SerializeField]
	private BossAttacks[] attacks;

	[SerializeField]
	private float phaseOneRange;


	private BossPhases currentPhase;


	[HideInInspector]
	public bool isAttacking;



	void Start () {


		isAttacking = false;
	
		currentPhase = new BossPhaseOne(this);

	}


	void Update () {
		currentPhase.HandleUpdate(this);
	}


	public void IfAttacking()
	{
		isAttacking = !isAttacking;
	}


	public void SpawnPrefab(int a)
	{
		GameObject prefab=Instantiate(attacks[a].AttackPrefab , attacks[a].AttackPoints.position, attacks[a].AttackPoints.rotation);
	}

	
	//private void LaunchAttack(Collider c)
	//{
	//	Collider[] cols = Physics.OverlapBox(c.bounds.center, c.bounds.extents, c.transform.rotation, LayerMask.GetMask("Hitbox", "Character"));
	//	Debug.Log(c.name);
	//	foreach (Collider col in cols)
	//	{
	//		if (col.transform.root == transform)
	//			continue;


	//		Debug.Log(col.name);

	//		float damage = 0;
	//		switch (c.name)
	//		{
	//			case "rLegHitBox":
	//				damage = 50;
	//				break;
	//			case "LeftFootHitbox":
	//				damage = 50;
	//				break;
	//			case "RightFootHitbox":
	//				damage = 50;
	//				break;
	//			case "lHandHitBox":
	//				damage = 50;
	//				break;
	//			case "rHandHitBox":
	//				damage = 50;
	//				break;
	//			default:
	//				Debug.Log("Unable to identify hitbox name");
	//				break;

	//		}
	//		col.SendMessageUpwards("TakeDamage", damage);
	//	}
	//}

	
	
}
