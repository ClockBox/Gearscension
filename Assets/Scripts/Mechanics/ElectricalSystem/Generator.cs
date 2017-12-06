using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Generator : ElectricalSwitch
{
    public Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!active && other.CompareTag("Player") && Input.GetButtonDown("Action"))
            Active = true;
    }
}