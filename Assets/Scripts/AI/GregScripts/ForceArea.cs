using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceArea : MonoBehaviour
{
	public float pushRadius;
	public float pushForce;
    public float lifeTime;
    public bool applyConstantForce;

    private Rigidbody tempRB;
    private float elapsedTime = 0.5f;

    private ChandelierTrap cT;

    void Start()
    {
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
            if (cols[i].tag == ("Chandelier"))
            {
                cT = cols[i].GetComponent<ChandelierTrap>();
                cT.DropChandelier();
            }

            tempRB = cols[i].attachedRigidbody;
            if (!cols[i].isTrigger && tempRB)
            {
                //tempRB.isKinematic = false;
                tempRB.AddExplosionForce(pushForce, transform.position, pushRadius);
            }
            if (cols[i].gameObject == GameManager.Player)
                PlayerState.grounded = false;

            AIStateManager temp;
            if (temp = cols[i].GetComponent<AIStateManager>())
                temp.TakeDamage(5);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pushRadius);
    } 
}
