using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIActions/Patrol")]
public class PatrolAction : AIActions {

	public override void Act(AIStateManager manager)
	{
		Patrol(manager);
	}
	private void Patrol(AIStateManager manager)
	{

		if ((Vector3.Distance(manager.pathTarget.position, manager.transform.position)) <= manager.stats.stopDistance)
		{
			manager.pathIndex++;


			if (manager.pathIndex >= manager.patrolPoints.Length)
				manager.pathIndex = 0;

			manager.pathTarget = manager.patrolPoints[manager.pathIndex];

			manager.pathAgent.destination=manager.pathTarget.position;
		}
	}

}
