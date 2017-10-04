using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIDecisions/EndSearchDecision")]

public class EndSearchDecision : AIDecisions {

	public override bool Decide(AIStateManager manager)
	{
		return EndSearch(manager);
	}

	private bool EndSearch (AIStateManager manager) {


		return (Vector3.Distance(manager.transform.position, manager.searchPosition.position) <= manager.stats.stopDistance);
	}
}
