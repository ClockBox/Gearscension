using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PressurePlate : ElectricalSwitch
{
    public GameObject weightedObject;
    public UnityEvent OnActiveStay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PushableObject>())
        {
            weightedObject = other.gameObject;
            Active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PushableObject>())
        {
            weightedObject = null;
            Active = false;
        }
    }
}