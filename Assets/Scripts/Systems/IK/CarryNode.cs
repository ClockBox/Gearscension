﻿using UnityEngine;
using System.Collections;

public class CarryNode : MonoBehaviour
{
    public static CarryNode Type;
    public Transform rightHand;
    public Transform leftHand;

    Rigidbody rb;
    Collider col;

    bool active = true;
    
    void Start ()
    {
        if (!Type)
            Type = this;

        if (!rightHand || !leftHand)
            Debug.LogError("Carry Node: " + gameObject.name + " cannot find hand positions");

        rb = transform.parent.GetComponent<Rigidbody>();
        if (!rb)
            Debug.LogError("Carry Node: " + gameObject.name + " Cannot find ridigbody in parent");

        col = transform.parent.GetComponent<Collider>();
        if (!col)
            Debug.LogError("Carry Node: " + gameObject.name + " Cannot find Collider in parent");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(rightHand.position, 0.05f);
        Gizmos.DrawSphere(leftHand.position, 0.05f);
    }

    public bool Active
    {
        set { active = value; }
        get { return active; }
    }

    public Rigidbody rigidBody
    {
        get {return rb; }
    }

    public Collider Collider
    {
        get { return col; }
    }
}
