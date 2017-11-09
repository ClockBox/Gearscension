using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIActions/Search")]
public class SearchAction : AIActions {

	public override void Act(AIStateManager manager)
	{
		Search(manager);
	}
	private void Search (AIStateManager manager) {
		manager.pathAgent.destination=manager.searchPosition.position;
	}
}
