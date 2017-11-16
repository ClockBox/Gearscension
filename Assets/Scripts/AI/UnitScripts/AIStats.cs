using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStats : MonoBehaviour {

	public float patrolSpeed;
	public float searchSpeed;
	public float engageSpeed;
	public float attackFrequency;
	public float lookRange;
	public float castSphereRadius;
	public float alertTimer;
	public float rangedRange;
	public float meleeRange;
	public float turnSpeed;
	public float rangedAttackDuration;
	public float meleeAttackDuration;
	public float armour;
	public float stunDuration;
	public float alertRadius;
	public float pursuitDistance;
	//public float lookHAngle;
	//public float lookVAngle;
	public float fovAngle;
	public float detectionRange;
	[HideInInspector]
	public float stopDistance = 3.5f;
}
