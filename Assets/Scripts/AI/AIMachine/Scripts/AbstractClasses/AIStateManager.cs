using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIStateManager : MonoBehaviour  {
	public Transform[] patrolPoints;
	public Transform[] visionPoints;
	public AIStates currentState;
	public AIStates alertedState;
	public AIStates remainState;
	public AIStates stunState;

	[HideInInspector]
	public float setFrequency;
	[HideInInspector]
	public Transform searchPosition;
	[HideInInspector]
	public AIStats stats;
	[HideInInspector]
	public UnitPathFinding pathAgent;
	[HideInInspector]
	public Transform pathTarget;
	[HideInInspector]
	public int pathIndex;
	[HideInInspector]
	public float stateTimeElapsed;
	[HideInInspector]
	public GameObject player;
	[HideInInspector]
	public bool callOnce=false;
	[HideInInspector]
	public bool isAlive;




	public void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		stats = GetComponent<AIStats>();
		pathAgent = GetComponent<UnitPathFinding>();
		pathIndex = 0;
		pathTarget = patrolPoints[pathIndex];
		pathAgent.travel(pathTarget.position);
		setFrequency = stats.attackFrequency;
		isAlive = true;
		StartEvents();

	}
	public void Update()
	{
		currentState.UpdateState(this);
		setFrequency += Time.deltaTime;


		//if (Input.GetKeyDown(KeyCode.G))
		//{
		//	TakeDamage(1);
		//	Debug.Log(stats.armour);
		//}
	}
	public void TransitionToState(AIStates nextState) {
		if (nextState != remainState)
		{
			currentState = nextState;
			OnStateExit();
		}
	}


	public abstract void RangedAttack();

	public abstract void MeleeAttack();

	public abstract void StartEvents();


	public void AlertOthers() {

		Collider[] cols;
		cols = Physics.OverlapSphere(transform.position, stats.alertRadius);
		for (int i = 0; i < cols.Length; i++)
		{
			if (cols[i].gameObject.tag == "Enemy")

				cols[i].GetComponent<AIStateManager>().Alerted();
		}
	}

	public abstract void Die();

	public abstract void CollisionEvents();

	public void Alerted() {
		TransitionToState(alertedState);
	}
	public void OnDrawGizmos()
	{
		if (currentState != null)
		{
			Gizmos.color = currentState.sceneGizmo;
			Gizmos.DrawWireSphere(this.gameObject.transform.position, 2);

		}
	}

	public bool checkTimeElapsed(float duration)
	{
		stateTimeElapsed += Time.deltaTime;
		return (stateTimeElapsed >= duration);
		
	}

	void OnStateExit() {
		stateTimeElapsed = 0;
		callOnce = false;
	}

	public void TakeDamage(float damage) {
 
		stats.armour--;
		if (stats.armour <= 0)
		{
			Stun();
		}
	}

	public void Stun()
	{
		Debug.Log("Stunned");
		stats.armour = 0;
		pathAgent.speed = 0;
		pathAgent.turnSpeed = 0;
		TransitionToState(stunState);
	}

	public void OnFreeze() {


	}
	public void OnThaw() {


	}

	private void OnCollisionEnter(Collision collision)
	{
		CollisionEvents();
	} 
}
