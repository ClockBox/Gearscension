using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIDecisions/ExitMeleeDecision")]

public class ExitMeleeDecision : AIDecisions {

	public override bool Decide(AIStateManager manager)
	{
		return Exit(manager);
	}

	private bool Exit(AIStateManager manager)
	{
		if (manager.checkTimeElapsed(manager.stats.meleeAttackDuration))
		{
			//manager.pathAgent.enabled = true;
			//manager.pathAgent.speed = manager.stats.engageSpeed;
			//manager.pathAgent.turnSpeed = manager.stats.turnSpeed;
			manager.setFrequency = 0;
			return true;
		}
		else
			return false;
	}
}
