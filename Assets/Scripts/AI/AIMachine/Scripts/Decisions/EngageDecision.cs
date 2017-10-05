using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIDecisions/EngageDecision")]

public class EngageDecision : AIDecisions {

	public override bool Decide(AIStateManager manager)
	{
		return Engage(manager);
	}


	// Update is called once per frame
	private bool Engage (AIStateManager manager) {

		if (Vector3.Distance(manager.transform.position, manager.player.transform.position) <= manager.stats.rangedRange)
		{
			manager.pathAgent.speed = 0;
			manager.pathAgent.turnSpeed = 0;

			return true;
		}
		else
	     return false;
	}
}
