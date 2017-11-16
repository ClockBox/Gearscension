using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIActions/Ranged")]

public class RangedAction : AIActions {

	public override void Act(AIStateManager manager)
	{
		Ranged(manager);
	}

	private void Ranged(AIStateManager manager)
	{
		manager.transform.LookAt(manager.player.transform);
		manager.RangedAttack();
	}
}
