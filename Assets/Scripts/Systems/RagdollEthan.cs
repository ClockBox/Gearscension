using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollEthan : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;

    float elapsedTime = 0;
    
	void Start ()
    {
        anim = transform.root.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
    }
	
	void Update ()
    {
        if (elapsedTime <= 5)
            elapsedTime += Time.deltaTime;
        else
        {
            anim.enabled = true;
            rb.freezeRotation = true;
            transform.rotation = Quaternion.identity;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rb.AddForce(new Vector3(Random.value, 10, Random.value) * rb.mass * 10);
            anim.enabled = false;
            rb.freezeRotation = false;
            elapsedTime = 0;
        }
    }
}

