using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPuller : MonoBehaviour {
    public float pullRadius = 0.5f;
    public float finishTime;
    public float lifeTime = 3f;
	// Use this for initialization
	void Start () {
        finishTime = 100f;
	}
	

     void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
        foreach (Collider collider in Physics.OverlapSphere(transform.position, pullRadius, LayerMask.GetMask("Hitbox")))
        {

            collider.GetComponent<Transform>().position = Vector3.Lerp(collider.GetComponent<Transform>().position, transform.position, lifeTime/finishTime);
        }
    } 

}
