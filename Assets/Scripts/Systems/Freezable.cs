using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezable : MonoBehaviour
{
    public GameObject IcePrefab;
    public GameObject climbNodePrefab;
    public Collider colliderBounds;
    public Animator animator;

    private GameObject IceBlock;
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
        scripts = GetComponents<MonoBehaviour>();
        if(transform.parent)
            parent = transform.parent;
    }

    void OnFreeze()
    {
        if (animator && colliderBounds)
        {
            Bounds bounds = colliderBounds.bounds;
            colliderBounds.attachedRigidbody.velocity = Vector3.zero;
            colliderBounds.attachedRigidbody.isKinematic = true;
            colliderBounds.enabled = false;
            animator.enabled = false;

            IceBlock = Instantiate(IcePrefab, bounds.center, Quaternion.identity);
            IceBlock.tag = "Pushable";

            Transform iceCube = IceBlock.transform.GetChild(0);
            iceCube.localScale = bounds.extents * 2;
            AddNodes(iceCube);

            for (int i = 0; i < scripts.Length; i++)
            {
                scripts[i].StopAllCoroutines();
                scripts[i].enabled = false;
            }

            transform.parent = IceBlock.transform;
        }
    }

    void OnThaw()
    {
        if (animator && colliderBounds)
        {
            animator.enabled = true;
            colliderBounds.attachedRigidbody.isKinematic = false;
            colliderBounds.enabled = true;
            
            for (int i = 0; i < scripts.Length; i++)
                scripts[i].enabled = true;

            transform.parent = parent;
            Destroy(IceBlock);
        }
    }

    void AddNodes(Transform iceCube)
    {
        // X Axis
        Instantiate(climbNodePrefab,
            iceCube.position + new Vector3(0.3f * iceCube.localScale.x, 0, iceCube.localScale.z / 2) + Vector3.up * iceCube.localScale.y / 2,
            new Quaternion(0, 1, 0, 0),
            IceBlock.transform);
        Instantiate(climbNodePrefab, 
            iceCube.position - new Vector3(0.3f * iceCube.localScale.x, 0, iceCube.localScale.z / 2) + Vector3.up * iceCube.localScale.y / 2, 
            Quaternion.identity,
            IceBlock.transform);
        for (float x = 1; x < iceCube.localScale.x; x += 0.5f)
        {
            Instantiate(climbNodePrefab, 
                iceCube.position + new Vector3(x * iceCube.localScale.x, 0, iceCube.localScale.z / 2) + Vector3.up * iceCube.localScale.y / 2,
                new Quaternion(0, 1, 0, 0),
                IceBlock.transform);
            Instantiate(climbNodePrefab,
                iceCube.position - new Vector3(x * iceCube.localScale.x, 0, iceCube.localScale.z / 2) + Vector3.up * iceCube.localScale.y / 2, 
                Quaternion.identity, 
                IceBlock.transform);
        }
        Instantiate(climbNodePrefab, 
            iceCube.position + new Vector3(-0.3f * iceCube.localScale.x, 0, iceCube.localScale.z / 2) + Vector3.up * iceCube.localScale.y / 2,
            new Quaternion(0, 1, 0, 0),
            IceBlock.transform);
        Instantiate(climbNodePrefab, 
            iceCube.position - new Vector3(-0.3f * iceCube.localScale.x, 0, iceCube.localScale.z / 2) + Vector3.up * iceCube.localScale.y / 2, 
            Quaternion.identity, 
            IceBlock.transform);

        // Z Axis
        Instantiate(climbNodePrefab,
            iceCube.position + new Vector3(iceCube.localScale.x/2, 0, 0.3f * iceCube.localScale.z) + Vector3.up * iceCube.localScale.y / 2,
            new Quaternion(0, -0.7071068f, 0, 0.7071068f),
            IceBlock.transform);
        Instantiate(climbNodePrefab,
            iceCube.position - new Vector3(iceCube.localScale.x / 2, 0, 0.3f * iceCube.localScale.z) + Vector3.up * iceCube.localScale.y / 2,
            new Quaternion(0, 0.7071068f, 0, 0.7071068f),
            IceBlock.transform);
        for (float z = 1; z < iceCube.localScale.z; z += 0.5f)
        {
            Instantiate(climbNodePrefab,
                iceCube.position + new Vector3(iceCube.localScale.x / 2, 0, z * iceCube.localScale.z) + Vector3.up * iceCube.localScale.y / 2,
                new Quaternion(0, -0.7071068f, 0, 0.7071068f),
                IceBlock.transform);
            Instantiate(climbNodePrefab,
                iceCube.position - new Vector3(iceCube.localScale.x / 2, 0, z * iceCube.localScale.z) + Vector3.up * iceCube.localScale.y / 2,
                new Quaternion(0, 0.7071068f, 0, 0.7071068f),
                IceBlock.transform);
        }
        Instantiate(climbNodePrefab,
            iceCube.position + new Vector3(iceCube.localScale.x / 2, 0, -0.3f * iceCube.localScale.z) + Vector3.up * iceCube.localScale.y / 2,
            new Quaternion(0, -0.7071068f, 0, 0.7071068f),
            IceBlock.transform);
        Instantiate(climbNodePrefab,
            iceCube.position - new Vector3(iceCube.localScale.x / 2, 0, -0.3f * iceCube.localScale.z) + Vector3.up * iceCube.localScale.y / 2,
            new Quaternion(0, 0.7071068f, 0, 0.7071068f),
            IceBlock.transform);

    }
}
