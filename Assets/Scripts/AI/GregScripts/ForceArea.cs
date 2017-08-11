using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceArea : MonoBehaviour
{
    static GameObject player;

	public float pushRadius;
	public float pushForce;
    public float lifeTime;
    public bool ApplyconstantForce;

    private Rigidbody tempRB;

    void Start()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject, lifeTime);

        if (!ApplyconstantForce)
            ApplyForce();
    }

    void FixedUpdate()
    {
        if (ApplyconstantForce)
            ApplyForce();
    }

    void ApplyForce()
    {
        Collider[] cols;
        cols = Physics.OverlapSphere(transform.position, pushRadius, LayerMask.GetMask("Debris", "Character"));
        for (int i = 0; i < cols.Length; i++)
        {
            if (!cols[i].isTrigger)
            {
                tempRB = cols[i].GetComponent<Rigidbody>();
                tempRB.AddExplosionForce(pushForce, transform.position, pushRadius);
                if (tempRB.CompareTag("Pushable"))
                    tempRB.isKinematic = false;
            }
            if (cols[i].gameObject == player)
                PlayerState.grounded = false;


            cols[i].SendMessageUpwards("TakeDamage", 20);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pushRadius);
    } 
}
