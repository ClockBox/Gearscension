﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIDecisions/SearchDecision")]

public class SearchDecision : AIDecisions {

	public override bool Decide(AIStateManager manager)
	{
		return Search(manager);
	}


	private bool Search (AIStateManager manager) {

		
		for (int i = 0; i < manager.visionPoints.Length; i++)
		{

			RaycastHit hit;
			Vector3 direction = (manager.player.transform.position - manager.visionPoints[i].position).normalized;
			Debug.DrawRay(manager.visionPoints[i].position,direction * manager.stats.lookRange, Color.red);

			if (Physics.SphereCast(manager.visionPoints[i].position, manager.stats.castSphereRadius, direction, out hit, manager.stats.lookRange)
				&& hit.collider.CompareTag("Player"))
			{
				if (manager.checkTimeElapsed(manager.stats.alertTimer))
				{
					manager.AlertOthers();
					return true;
				}
			}
		}
		return false;
	}
}
