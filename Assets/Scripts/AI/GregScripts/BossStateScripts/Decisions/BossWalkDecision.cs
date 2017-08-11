using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BossDecisions/WalkDecision")]

public class BossWalkDecision : AIDecisions {

	public override bool Decide(AIStateManager manager)
	{
		bool decidedState = makeDecision(manager);
		return decidedState;

	}

	private bool makeDecision(AIStateManager manager) {

		bool decision=true;




		return decision;


	}

}