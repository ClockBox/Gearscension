using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIDecisions/StunDecision")]

public class StunDecision : AIDecisions {

	public override bool Decide(AIStateManager manager)
	{
		return Restart(manager);
	}
	private bool Restart(AIStateManager manager)
	{
		if (manager.checkTimeElapsed(manager.stats.stunDuration))
		{
			manager.pathAgent.speed = manager.stats.engageSpeed;
			manager.pathAgent.turnSpeed = manager.stats.turnSpeed;
			manager.stats.armour =1;
			return true;
		}
		else
			return false;
	}
}
