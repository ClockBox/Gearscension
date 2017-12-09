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
        //if (!manager.pathAgent.enabled)
        //{
        //	manager.pathAgent.enabled = true;
        //	manager.pathAgent.speed = manager.stats.engageSpeed;
        //}

        if (!manager.player)
            return;

        if (manager.pathAgent.isOnNavMesh)
        {
            manager.pathAgent.destination = manager.player.transform.position;
            if (Vector3.Distance(manager.transform.position, manager.player.transform.position) <= manager.stats.pursuitDistance)
            {
                manager.pathAgent.isStopped = true;
            }
            else
            {
                manager.pathAgent.isStopped = false;
            }
        }
	}
}
