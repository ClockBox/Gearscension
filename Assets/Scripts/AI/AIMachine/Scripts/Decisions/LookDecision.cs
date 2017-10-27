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
		//	for (int i = 0; i < manager.visionPoints.Length; i++)
		//	{

		//		RaycastHit hit;

		//		Debug.DrawRay(manager.visionPoints[i].position, manager.visionPoints[i].forward.normalized * manager.stats.lookRange, Color.green);

		//		if (Physics.SphereCast(manager.visionPoints[i].position, manager.stats.castSphereRadius, manager.visionPoints[i].forward, out hit, manager.stats.lookRange)
		//			&& hit.collider.CompareTag("Player"))
		//		{
		//			manager.searchPosition = hit.collider.transform;
		//			manager.pathAgent.speed = manager.stats.searchSpeed;
		//			return true;
		//		}


		//	}
		//float dotRange= Vector3.Dot(manager.transform.forward, manager.player.transform.position - manager.transform.position);
		//float dotX= Vector3.Dot(manager.transform.right, manager.player.transform.position - manager.transform.position);
		//float dotY = Vector3.Dot(manager.transform.up, manager.player.transform.position - manager.transform.position);
		float angle = Vector3.Angle(manager.player.transform.position - manager.transform.position, manager.transform.forward);
		if (Vector3.Distance(manager.transform.position, manager.player.transform.position) <= manager.stats.detectionRange || angle <= manager.stats.fovAngle)
		{

			for (int i = 0; i < manager.visionPoints.Length; i++)
			{
				Vector3 playerPos = new Vector3(manager.player.transform.position.x, manager.player.transform.position.y + 1.25f, manager.player.transform.position.z);
				Vector3 direction = (playerPos - manager.visionPoints[i].position).normalized;
				RaycastHit hit;
				Debug.DrawRay(manager.visionPoints[i].position, direction * manager.stats.lookRange, Color.green);

				if (Physics.Raycast(manager.visionPoints[i].position,  direction, out hit, manager.stats.lookRange))
					{
					if (hit.collider.CompareTag("Player"))
					{
					manager.searchPosition = hit.collider.transform;
					manager.pathAgent.speed = manager.stats.searchSpeed;
					return true;
					}
				}

			}

		}

		return false;
	}

	
}
