using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIActions/Attack")]

public class AttackAction : AIActions {

	public override void Act(AIStateManager manager)
	{
		Attack(manager);
	}

	private void Attack (AIStateManager manager) {
		manager.stats.attackSpeed += Time.deltaTime;
		if (manager.stats.attackSpeed >= manager.setSpeed)
		{
			manager.RangedAttack();
			manager.stats.attackSpeed = 0;
		}
		else
			manager.pathAgent.travel(manager.player.transform.position);
	}
}
