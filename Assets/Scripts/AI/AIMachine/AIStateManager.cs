using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIStateManager : MonoBehaviour {
	public AIStates currentState;
	public AIStates remainState;

	public void TransitionToState(AIStates nextState) {
		if (nextState != remainState)
		{
			currentState = nextState;
		}
	}

	public void OnDrawGizmos()
	{
		if (currentState != null)
		{
			Gizmos.color = currentState.sceneGizmo;
			Gizmos.DrawWireSphere(this.gameObject.transform.position, 2);

		}
	}
}
