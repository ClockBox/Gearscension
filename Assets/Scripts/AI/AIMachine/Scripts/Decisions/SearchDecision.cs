using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AIMachine/ScriptableObjects/AIDecisions/SearchDecision")]

public class SearchDecision : AIDecisions {

	public override bool Decide(AIStateManager manager)
	{
		return Search(manager);
	}


	private bool Search (AIStateManager manager) {

		float dotRange = Vector3.Dot(manager.transform.forward, manager.player.transform.position - manager.transform.position);
		float dotX = Vector3.Dot(manager.transform.right, manager.player.transform.position - manager.transform.position);
		float dotY = Vector3.Dot(manager.transform.up, manager.player.transform.position - manager.transform.position);
		if (Vector3.Distance(manager.transform.position, manager.player.transform.position) <= manager.stats.detectionRange)
		{
			return true;
		}
		if (dotRange > 0 && dotRange <= manager.stats.lookRange && Mathf.Abs(dotX) <= manager.stats.lookHAngle && Mathf.Abs(dotY) <= manager.stats.lookVAngle)
		{
			for (int i = 0; i < manager.visionPoints.Length; i++)
			{

				RaycastHit hit;
				Vector3 playerPos = new Vector3(manager.player.transform.position.x, manager.player.transform.position.y + 1.25f, manager.player.transform.position.z);
				Vector3 direction = (playerPos - manager.visionPoints[i].position).normalized;
			   Debug.DrawRay(manager.visionPoints[i].position,direction * manager.stats.lookRange, Color.red);

				if (Physics.SphereCast(manager.visionPoints[i].position, manager.stats.castSphereRadius, direction, out hit, manager.stats.lookRange)
					&& hit.collider.CompareTag("Player"))
				{
					if (manager.checkTimeElapsed(manager.stats.alertTimer))
					{
						manager.AlertOthers();
						manager.pathAgent.speed = manager.stats.engageSpeed;
						return true;
					}
				}


			}
		}
		return false;
		
	}
}
