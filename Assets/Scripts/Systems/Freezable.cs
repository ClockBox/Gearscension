using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Freezable : MonoBehaviour
{
    public GameObject IcePrefab;
    public Collider colliderBounds;
    public Animator animator;

    private Rigidbody rb;
    private GameObject iceBlock;
    private Transform parent;
    private MonoBehaviour[] scripts;

    private bool pk_freeze;
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
    public void ToggleFreeze()
    {
        Freeze = !Freeze;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        scripts = GetComponents<MonoBehaviour>();
        if (transform.parent)
            parent = transform.parent;
    }

    private void FixedUpdate()
    {
        if (rb) rb.AddForce(Vector3.down * 9.81f * rb.mass);
    }

    void OnFreeze()
    {
        if (colliderBounds)
        {
            NavMeshAgent agent;
            if (agent = GetComponent<NavMeshAgent>())
                agent.enabled = false;

            Bounds bounds = colliderBounds.bounds;
            colliderBounds.enabled = false;

            if (animator)
                animator.enabled = false;

            iceBlock = Instantiate(IcePrefab, bounds.center, transform.rotation);

            Transform iceCube = iceBlock.transform.GetChild(0);
            iceCube.localScale = bounds.extents * 2;

            for (int i = 0; i < scripts.Length; i++)
            {
                scripts[i].StopAllCoroutines();
                scripts[i].enabled = false;
            }
            transform.parent = iceBlock.transform;

            if (colliderBounds.attachedRigidbody)
            {
                colliderBounds.attachedRigidbody.velocity = Vector3.zero;
                colliderBounds.attachedRigidbody.isKinematic = true;
            }
        }
    }

    void OnThaw()
    {
        if (colliderBounds)
        {
            NavMeshAgent agent;
            if (agent = GetComponent<NavMeshAgent>())
                agent.enabled = true;

            if (animator)
                animator.enabled = true;

            if (colliderBounds.attachedRigidbody)
                colliderBounds.attachedRigidbody.isKinematic = false;

            colliderBounds.enabled = true;

            for (int i = 0; i < scripts.Length; i++)
                scripts[i].enabled = true;

            transform.parent = parent;
            Destroy(iceBlock);
        }
    }
}
