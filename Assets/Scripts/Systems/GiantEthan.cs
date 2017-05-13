using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantEthan : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>());
        anim = GetComponent<Animator>();
	}
	
	void Update ()
    {
        rb.velocity = transform.forward * 100;
        transform.Rotate(-Vector3.up * 4.5f * Time.deltaTime);
        anim.SetFloat("Speed", 1);
	}
}
