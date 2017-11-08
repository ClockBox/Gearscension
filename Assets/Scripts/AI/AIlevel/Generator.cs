using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Generator : MonoBehaviour
{
    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;

    private bool active = false;
    public bool Active
    {
        get { return active; }
        set
        {
            active = value;
            if (active) OnActivate.Invoke();
            else OnDeactivate.Invoke();
        }
    }
	public void Activate()
    {
        active = true;
    }
    public void Deactivate()
    {
        active = true;
    }
    public void Invert()
    {
        active = !active;
    }
}
