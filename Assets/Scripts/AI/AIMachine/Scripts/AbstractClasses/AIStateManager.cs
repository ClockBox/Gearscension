using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


public abstract class AIStateManager : MonoBehaviour
{
	public Transform[] patrolPoints;
	public Transform[] visionPoints;
	public AIStates currentState;
	public AIStates alertedState;
	public AIStates remainState;
	public AIStates stunState;
	public GameObject smokePrefab;
	public GameObject firePrefab;
	public Transform exhaust;

    public GameObject[] BreakingPieces;
    public GameObject[] DestroyPieces;

    [HideInInspector]
	public float setFrequency;
	[HideInInspector]
	public Transform searchPosition;
	[HideInInspector]
	public AIStats stats;
	[HideInInspector]
	public NavMeshAgent pathAgent;
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

    protected Rigidbody rb; 

	private bool isAttacked = false;

	public void Start()
	{
        player = GameManager.Player.gameObject;
        rb = GetComponent<Rigidbody>();
        stats = GetComponent<AIStats>();
		pathAgent = GetComponent<NavMeshAgent>();
		pathIndex = 0;
        if (patrolPoints.Length > 0)
        {
            pathTarget = patrolPoints[pathIndex];
            pathAgent.destination = pathTarget.position;
        }
		setFrequency = stats.attackFrequency;
		isAlive = true;
		StartEvents();

	}
	public virtual void Update()
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

	public void Die(float delay)
	{
		if (isAlive)
		{
			GetComponent<Collider>().enabled = false;
            Break();
            Destroy(gameObject, delay);
			isAlive = false;
		}
	}

    public void Break()
    {
        for (int i = 0; i < BreakingPieces.Length; i++)
        {
            Collider[] temp = BreakingPieces[i].GetComponents<Collider>();
            if (temp.Length > 0)
            {
                for (int t = 0; t < temp.Length; t++)
                {
                    temp[t].enabled = true;
                }
            }
            else BreakingPieces[i].AddComponent<BoxCollider>();

            Rigidbody tempRB;
            if ((tempRB = BreakingPieces[i].GetComponent<Rigidbody>()))
                tempRB.isKinematic = false;
            else BreakingPieces[i].AddComponent<Rigidbody>();

            BreakingPieces[i].transform.parent = null;
            Destroy(BreakingPieces[i].gameObject, UnityEngine.Random.Range(8, 10));
        }

        for (int i = 0; i < DestroyPieces.Length; i++)
            Destroy(DestroyPieces[i]);

        transform.parent = null;
    }


    public void Alerted()
    {
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

	void OnStateExit()
    {
		stateTimeElapsed = 0;
		callOnce = false;
	}

	public void TakeDamage(float damage)
    {
		if (stats.armour > 0){
			stats.armour--;
		if (stats.armour <= 0)
			{
				Stun();
			}
		}
	}

	public void Stun()
	{
		Debug.Log(this + " was Stunned");
		stats.armour = 0;
		if (pathAgent.enabled)
		{
			pathAgent.speed = 0;
			pathAgent.angularSpeed = 0;
		}
		if(GetComponentInChildren<AIBreakable>())
		GetComponentInChildren<AIBreakable>().Breaks();
		TransitionToState(stunState);
	}

    public void Magnitize()
    {
        TransitionToState(stunState);
        Debug.Log(this + " was Magnitized");
        pathAgent.enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void Demagnitize()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        pathAgent.enabled = true;
        TransitionToState(alertedState);
        Debug.Log(this + " was Demagnitize");
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Sword")&&isAttacked==false)
		{
			Debug.Log("HIT by sword");
			TakeDamage(5f);
			isAttacked = true;
			Invoke("ResetAttacked", 0.8f);
		}
	}
	private void ResetAttacked()
	{
		isAttacked = false;
	}
}
