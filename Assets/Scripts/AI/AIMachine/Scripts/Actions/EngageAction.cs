using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIActions/Engage")]

public class EngageAction : AIActions {

	public override void Act(AIStateManager manager)
	{
		Engage(manager);
	}

	private void Engage(AIStateManager manager)
	{
		if (!manager.pathAgent.enabled)
			manager.pathAgent.enabled = true;
		manager.pathAgent.travel(manager.player.transform.position);
	}
}
