using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFreezable
{
    float freezeResistance { get; set;}
    bool isFrozen { get; set; }

    void OnFreeze();
    void OnThaw();
}

public interface IMagnetic
{
    float magnetizingForce { get; set; }
    bool isMagnitized { get; set; }

    void OnMagnetized();
    void OnDeMagnetized();
}

public interface IBreakable
{
    float durability { get; set; }
    bool isBroken { get; set; }

    void TakeDamage();
    void OnBreak();
}