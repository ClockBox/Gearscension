using UnityEngine;
using System.Collections;

public class HandNode : MonoBehaviour
{
    public static HandNode Type;
    public Transform rightHand;
    public Transform leftHand;
    
    public Rigidbody rb;
    public Collider col;

    bool active = true;
    
    void Start ()
    {
        if (!Type)
            Type = this;
        if(!rb)
            rb = transform.parent.GetComponent<Rigidbody>();
        if(!col)
            col = transform.parent.GetComponent<Collider>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(rightHand.position, 0.05f);
        Gizmos.DrawSphere(leftHand.position, 0.05f);
    }

    public bool Active
    {
        set { active = value; }
        get { return active; }
    }

    public Rigidbody rigidBody
    {
        get {return rb; }
    }

    public Collider Collider
    {
        get { return col; }
    }
}
