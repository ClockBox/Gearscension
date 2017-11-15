using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FreezableEnemy : Freezable
{
    public Animator animator;
    protected MonoBehaviour[] scripts;

    protected override void Awake()
    {
        scripts = GetComponents<MonoBehaviour>();
    }

    protected override void OnFreeze()
    {
        base.OnFreeze();
        if (colliderBounds)
        {
            NavMeshAgent agent;
            if (agent = GetComponent<NavMeshAgent>())
                agent.enabled = false;

            if (animator)
                animator.enabled = false;

            SphereCollider temp = iceBlock.transform.GetChild(0).gameObject.AddComponent<SphereCollider>();
            temp.radius = 0.51f;

            for (int i = 0; i < scripts.Length; i++)
            {
                scripts[i].StopAllCoroutines();
                scripts[i].enabled = false;
            }

            if (colliderBounds.attachedRigidbody)
                colliderBounds.attachedRigidbody.isKinematic = false;

            transform.parent = iceBlock.transform;
        }
    }

    protected override void OnThaw()
    {
        base.OnThaw();
        if (colliderBounds)
        {
            NavMeshAgent agent;
            if (agent = GetComponent<NavMeshAgent>())
                agent.enabled = true;

            if (animator)
                animator.enabled = true;

            for (int i = 0; i < scripts.Length; i++)
                scripts[i].enabled = true;
        }
    }
}
