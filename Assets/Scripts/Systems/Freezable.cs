using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Freezable : MonoBehaviour
{
    public GameObject IcePrefab;
    public Collider colliderBounds;

    public bool climbable;
    public bool pushable;

    protected Rigidbody rb;
    protected IceCube iceBlock;
    protected Transform parent;

    protected bool pk_freeze;
    public bool Freeze
    {
        get { return pk_freeze; }
        set
        {
            pk_freeze = value;
            if (value) OnFreeze();
            else OnThaw();
        }
    }
    protected void ToggleFreeze()
    {
        Freeze = !Freeze;
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (transform.parent)
            parent = transform.parent;
    }

    protected void FixedUpdate()
    {
        if (rb) rb.AddForce(Vector3.down * 9.81f * rb.mass);
    }

    protected virtual void OnFreeze()
    {
        if (colliderBounds)
        {
            Bounds bounds = colliderBounds.bounds;
            colliderBounds.enabled = false;

            iceBlock = Instantiate(IcePrefab, bounds.center, Quaternion.identity).GetComponent<IceCube>();
            iceBlock.pushable = pushable;
            iceBlock.climbable = climbable;

            Transform iceCube = iceBlock.transform.GetChild(0);
            iceCube.tag = tag;
            iceCube.localScale = bounds.extents * 2;
            transform.parent = iceBlock.transform;

            if (colliderBounds.attachedRigidbody)
            {
                colliderBounds.attachedRigidbody.velocity = Vector3.zero;
                colliderBounds.attachedRigidbody.isKinematic = true;
            }
        }
    }

    protected virtual void OnThaw()
    {
        if (colliderBounds)
        {
            if (colliderBounds.attachedRigidbody)
                colliderBounds.attachedRigidbody.isKinematic = false;

            colliderBounds.enabled = true;

            transform.parent = parent;
            Destroy(iceBlock.gameObject);
        }
    }
}
