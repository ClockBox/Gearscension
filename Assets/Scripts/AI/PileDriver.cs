using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileDriver : MonoBehaviour {
    public bool retracted = true;
    Rigidbody rb;
    Transform bossTransform;
    Transform originalPos;
    float force;
    float mass;
    private void Start()
    {
        
        originalPos = transform;
        rb = GetComponent<Rigidbody>();
        bossTransform = GameObject.FindGameObjectWithTag("Boss").transform;
        mass = rb.mass;
    }
    private void FixedUpdate()
    {
        bossTransform = GameObject.FindGameObjectWithTag("Boss").transform;
    
    }

 
	public void Activate()
    {
        if (retracted)
        {

            force = calculateForce(transform, bossTransform,1);
            transform.LookAt(bossTransform);
            rb.AddForce(transform.forward* force);
            retracted = false;
        }
        else if (!retracted)
        {
            force = calculateForce(transform, originalPos, 1);

            transform.LookAt(originalPos);
            rb.AddForce(transform.forward * force,ForceMode.Impulse);
            retracted = true;

        }
        Invoke("resetSpeed", 0.5f);
    }

    float calculateForce(Transform a, Transform b, float time)
    {
        Debug.Log(retracted);
        float sforce;
        float acceleration = Vector3.Distance(a.position, b.position) * 2 / time;
        sforce = mass * acceleration*acceleration;

        return sforce;
    }
    void resetSpeed()
    {
        rb.velocity = Vector3.zero;
    }
}
