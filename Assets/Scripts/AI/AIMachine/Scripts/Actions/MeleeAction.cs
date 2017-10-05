using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIActions/Melee")]

public class MeleeAction : AIActions  {

	public override void Act(AIStateManager manager)
	{
		Melee(manager);
	}

	private void Melee (AIStateManager manager) {
		manager.MeleeAttack();

	}
}
