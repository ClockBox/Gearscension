using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElectricalSwitch : ElectricalObject
{
    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;
    
    public override void Activate()
    {
        base.Activate();
        OnActivate.Invoke();
    }
    public override void Deactivate()
    {
        base.Deactivate();
        OnDeactivate.Invoke();
    }
}
