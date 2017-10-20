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

		if (Vector3.Distance(manager.transform.position, manager.searchPosition.position) <= manager.stats.stopDistance)
		{
			if (manager.checkTimeElapsed(manager.stats.alertTimer))
			{
				manager.pathAgent.speed = manager.stats.patrolSpeed;
				manager.pathAgent.travel(manager.pathTarget.position);
				return true;
			}
		}
		return false;

		
	}
	
}
