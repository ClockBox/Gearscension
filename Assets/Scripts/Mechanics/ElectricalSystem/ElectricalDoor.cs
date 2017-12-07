using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalDoor : ElectricalObject
{
    public override void Activate()
    {
        base.Activate();
        anim.SetBool("Activated", true);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        anim.SetBool("Activated", false);
    }
}
