using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIActions/Stun")]

public class StunAction : AIActions {

	public override void Act(AIStateManager manager)
	{
		Stun(manager);
	}

	private void Stun(AIStateManager manager)
	{
		if (!manager.pathAgent.isStopped)
		{
			manager.pathAgent.isStopped = true;
		}
	}
}
