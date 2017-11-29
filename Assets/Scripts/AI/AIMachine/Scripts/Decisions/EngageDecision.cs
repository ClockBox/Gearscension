using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIDecisions/EngageDecision")]

public class EngageDecision : AIDecisions {

	public override bool Decide(AIStateManager manager)
	{
		return Engage(manager);
	}


	private bool Engage (AIStateManager manager) {

        if (!manager.player)
            return true;
		if (Vector3.Distance(manager.transform.position, manager.player.transform.position) <= manager.stats.rangedRange&&manager.setFrequency >=manager.stats.attackFrequency)
			return true;
		else
	     return false;
	}
}
