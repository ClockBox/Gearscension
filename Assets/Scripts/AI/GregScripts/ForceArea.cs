using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ForceType
{
    Magnetic,
    Explosive
}

public class ForceArea : MonoBehaviour
{
    public ForceType type;
	public float pushForce;
    public float lifeTime;
    public bool applyConstantForce;

    private SphereCollider AreaCheck;

    void Awake()
    {
        AreaCheck = GetComponent<SphereCollider>();
        Destroy(gameObject, lifeTime);
    }

    void ApplyForce(Collider refObject)
    {
        Rigidbody tempRB;
        tempRB = refObject.attachedRigidbody;
        if (refObject.isTrigger && tempRB)
            tempRB.AddExplosionForce(pushForce, transform.position, AreaCheck.radius);
    }

    private void AffectObject(Collider refObject)
    {
        if (refObject.tag == ("Chandelier"))
        {
            ChandelierTrap cT;
            if(cT = refObject.GetComponent<ChandelierTrap>())
                cT.DropChandelier();
        }

        else if (refObject.tag == ("Enemy"))
        {
            AIStateManager temp;
            if (temp = refObject.GetComponent<AIStateManager>())
            {
                if (type == ForceType.Explosive)
                    temp.TakeDamage(5);
                else if (type == ForceType.Magnetic)
                    temp.Magnitize();
            }
        }
    }

    private void UnaffectObject(Collider refObject)
    {
        if (refObject.tag == ("Enemy"))
        {
            AIStateManager temp;
            if ((type == ForceType.Magnetic) && (temp = refObject.GetComponent<AIStateManager>()))
                temp.Demagnitize();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AffectObject(other);
        ApplyForce(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if(applyConstantForce)
            ApplyForce(other);
    }

    private void OnTriggerExit(Collider other)
    {
        UnaffectObject(other);
    }

    private void OnDestroy()
    {
        Collider[] cols = Physics.OverlapSphere(AreaCheck.bounds.center, AreaCheck.radius);
        for (int i = 0; i < cols.Length; i++)
            UnaffectObject(cols[i]);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AreaCheck.radius);
    } 
}
