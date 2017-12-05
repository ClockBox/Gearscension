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
        if (!manager.player)
            return;

        Vector3 flatLookDirection = manager.player.transform.position;
        flatLookDirection.y = manager.transform.position.y;

        manager.transform.LookAt(flatLookDirection);
		manager.RangedAttack();
	}
}
