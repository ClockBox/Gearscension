using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIActions/Attack")]

public class AttackAction : AIActions {

	public override void Act(AIStateManager manager)
	{
		StartAttack(manager);
	}

	// Update is called once per frame
	private void StartAttack (AIStateManager manager) {
		if(manager.pathAgent.enabled)
		manager.pathAgent.enabled = false;
	}
}
