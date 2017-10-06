using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIDecisions/LookDecision")]

public class LookDecision : AIDecisions {

	public override bool Decide(AIStateManager manager)
	{
		return Look(manager);
	}

	private bool Look(AIStateManager manager)
	{
		for (int i = 0; i < manager.visionPoints.Length; i++)
		{

			RaycastHit hit;

			Debug.DrawRay(manager.visionPoints[i].position, manager.visionPoints[i].forward.normalized * manager.stats.lookRange, Color.green);

			if (Physics.SphereCast(manager.visionPoints[i].position, manager.stats.castSphereRadius, manager.visionPoints[i].forward, out hit, manager.stats.lookRange)
				&& hit.collider.CompareTag("Player"))
			{
				manager.searchPosition = hit.collider.transform;
				manager.pathAgent.speed = manager.stats.searchSpeed;
				return true;
			}
		}
		return false;
	}
}
