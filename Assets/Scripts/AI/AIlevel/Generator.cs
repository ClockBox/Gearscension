using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Generator : MonoBehaviour
{
    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;

    public GameObject[] lights;

    private bool active = false;
    private bool Active
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
        Active = true;
        ToggleLights();
    }
    public void Deactivate()
    {
        Active = false;
        ToggleLights();
    }
    public void Invert()
    {
        Active = !active;
        ToggleLights();
    }

    public void ToggleLights()
    {
        for (int i = 0; i < lights.Length; i++)
            lights[i].SetActive(!lights[i].activeSelf);
    }
}
