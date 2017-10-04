using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : AIStateManager {



	public override void RangedAttack()
	{
		pathAgent.travel(transform.position);

	}
	public override void MeleeAttack()
	{
	}
	public override void AlertOthers()
	{
	}
}
