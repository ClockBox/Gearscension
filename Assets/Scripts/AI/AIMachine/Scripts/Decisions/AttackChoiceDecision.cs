using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIDecisions/AttackChoiceDecision")]

public class AttackChoiceDecision : AIDecisions {

	public override bool Decide(AIStateManager manager)
	{
		return ChooseAttack(manager);
	}

	private bool ChooseAttack (AIStateManager manager) {

		if (Vector3.Distance(manager.transform.position, manager.player.transform.position) <= manager.stats.meleeRange)
		{

			return true;
		}
		else
		{
		
			manager.pathAgent.speed = 0;
			//manager.pathAgent.turnSpeed = 0;
			//manager.pathAgent.enabled = false;
			return false;
		}
	}
}
