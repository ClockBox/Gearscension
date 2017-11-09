using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceArea : MonoBehaviour
{
    static GameObject player;

	public float pushRadius;
	public float pushForce;
    public float lifeTime;
    public bool applyConstantForce;

    private Rigidbody tempRB;
    private float elapsedTime = 0.5f;

    void Start()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject, lifeTime);

        if (!applyConstantForce)
            ApplyForce();
    }

    void FixedUpdate()
    {
        if (applyConstantForce && elapsedTime >= 0.5f)
            ApplyForce();
        else elapsedTime += Time.deltaTime;
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
            cols[i].GetComponent<AIStateManager>().TakeDamage(5);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pushRadius);
    } 
}
