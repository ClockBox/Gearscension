using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Generator : ElectricalSwitch
{
    private void OnTriggerStay(Collider other)
    {
        if (!active && other.CompareTag("Player") && Input.GetButtonDown("Action"))
            Active = true;
    }
}