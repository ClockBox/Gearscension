using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossPhases {

	public void HandleUpdates(Grim boss) {
		HandleMovement(boss);
		HandleAction(boss);
	}
	public abstract void HandleMovement(Grim boss);
	public abstract void HandleAction(Grim boss);
}

public class BossPhaseOne : BossPhases
{
	public override void HandleMovement(Grim boss)
	{
	
	}

	public override void HandleAction(Grim boss) {
	}
}

public class BossPhaseTwo : BossPhases
{
	public override void HandleMovement(Grim boss)
	{
	}

	public override void HandleAction(Grim boss)
	{
	}
}

public class BossPhaseThree : BossPhases
{
	public override void HandleMovement(Grim boss)
	{
	}

	public override void HandleAction(Grim boss)
	{
	}
}
