using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIActions/Attack")]

public class AttackAction : AIActions {

	public override void Act(AIStateManager manager)
	{
		StartAttack(manager);
	}

	private void StartAttack (AIStateManager manager) {

	}
}
