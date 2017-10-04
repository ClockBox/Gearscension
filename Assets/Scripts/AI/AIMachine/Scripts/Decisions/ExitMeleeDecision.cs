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
		return manager.checkTimeElapsed(manager.stats.meleeAttackDuration );
	}
}
