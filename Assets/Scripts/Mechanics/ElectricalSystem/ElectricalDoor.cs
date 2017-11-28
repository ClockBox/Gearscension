using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalDoor : ElectricalObject
{
    private Animator anim;

	void Awake ()
    {
        anim = GetComponent<Animator>();
    }

    public override void Activate()
    {
        base.Activate();
        anim.SetBool("Open", true);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        anim.SetBool("Open", false);
    }
}
