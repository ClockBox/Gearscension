using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierTrap : MonoBehaviour
{
    RigidbodyConstraints rbc;
    Rigidbody rb;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Projectile")
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
        }
    }
}
