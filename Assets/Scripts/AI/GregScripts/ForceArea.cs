using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceArea : MonoBehaviour
{
    static GameObject player;

	public float pushRadius;
	public float pushForce;
    public float lifeTime;

    void Start()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        Collider[] cols;
        cols = Physics.OverlapSphere(transform.position, pushRadius, LayerMask.GetMask("Hitbox"));
        for (int i = 0; i < cols.Length; i++)
        {
            if(!cols[i].isTrigger)
                cols[i].GetComponent<Rigidbody>().AddExplosionForce(pushForce, transform.position, pushRadius);
            if (cols[i].gameObject == player)
                PlayerState.grounded = false;
        }
    }
}
