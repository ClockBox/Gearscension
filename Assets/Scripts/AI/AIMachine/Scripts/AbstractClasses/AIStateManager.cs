using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIStateManager : MonoBehaviour {
	public Transform[] patrolPoints;
	public Transform[] visionPoints;
	public AIStates currentState;
	public AIStates alertedState;
	public AIStates remainState;

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



	public void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		stats = GetComponent<AIStats>();
		pathAgent = GetComponent<UnitPathFinding>();
		pathIndex = 0;
		pathTarget = patrolPoints[pathIndex];
		pathAgent.travel(pathTarget.position);
		setFrequency = stats.attackFrequency;
	}
	public void Update()
	{
		currentState.UpdateState(this);
		setFrequency += Time.deltaTime;
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

	public abstract void AlertOthers();

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
	}


}
