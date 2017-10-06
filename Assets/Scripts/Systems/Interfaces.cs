using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFreezable
{
	float freezeResistance { get; set; }
	bool isFrozen { get; set; }

	void OnFreeze();
	void OnThaw();
}

public interface IBreakable
{
}
public interface IMagnetic
{
}
