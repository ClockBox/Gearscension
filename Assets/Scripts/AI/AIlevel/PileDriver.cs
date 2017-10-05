using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileDriver : MonoBehaviour {
    public bool retracted = true;
    public float speed = 20f;
    Rigidbody rb;
    Transform bossTransform;
    Vector3 originalPos;
    bool activated = false;
    
    private void Start()
    {
        
        originalPos = transform.position;
        rb = GetComponent<Rigidbody>();
        bossTransform = GameObject.FindGameObjectWithTag("Boss").transform;
    }
    private void FixedUpdate()
    {
        if(!activated)
        bossTransform = GameObject.FindGameObjectWithTag("Boss").transform;
        else if (activated) {
            if (!retracted)
            {
                Debug.Log("EHIEJIE");
                transform.position = Vector3.MoveTowards(transform.position, bossTransform.position, speed * Time.deltaTime);

                Vector3 point = bossTransform.position;
                point.y = transform.position.y;

                transform.LookAt(point);

            }
            else

            {
                Debug.Log("fefefe");

                transform.position = Vector3.MoveTowards(transform.position, originalPos, speed * Time.deltaTime);

               
            }
        }

      
    }

 
	public void Activate()
    {
        activated = true;
        retracted = !retracted;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Boss")
        {
            other.gameObject.GetComponent<gregPhaseOne>().hitByPD();
        }
    } 
}
