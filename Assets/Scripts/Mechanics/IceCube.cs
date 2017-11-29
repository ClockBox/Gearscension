using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCube : MonoBehaviour
{
    public bool climbable;
    public bool pushable;
    public bool pushing;

    public GameObject climbNodePrefab;
    public GameObject pushNodePrefab;

    private Collider cubeBounds;
    
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start ()
    {
        cubeBounds = transform.GetChild(0).GetComponent<Collider>();
        Vector3 extents = cubeBounds.bounds.extents;
        if (extents.x > extents.z)
            extents.x = extents.z;
        else
            extents.z = extents.x;
        transform.GetChild(0).localScale = extents * 2;
        
        for (int r = 0; r < 4; r++)
        {
            Quaternion rotation = transform.rotation * Quaternion.Euler(0, 90 * r, 0);
            if (climbable && climbNodePrefab && extents.y > 1)
            {
                for (int i = (int)-extents.x; i < extents.x; i++)
                {
                    IKPositionNode node = Instantiate(climbNodePrefab,
                        transform.position + rotation * new Vector3(extents.x, extents.y, i),
                        rotation * Quaternion.Euler(0, -90, 0),
                        transform).GetComponent<IKPositionNode>();
                    node.siblingNodes = true;
                }
            }

            if (pushable && pushNodePrefab)
            {
                Instantiate(pushNodePrefab,
                    transform.position + rotation * new Vector3(extents.x, Mathf.Min(extents.y, -extents.y + 1.3f), 0),
                    rotation * Quaternion.Euler(0, -90, 0),
                    transform);
            }
        }
    }

    private void FixedUpdate()
    {
        if (pushing)
            rb.isKinematic = false;
        else
        {
            Collider[] cols = Physics.OverlapBox(transform.position, cubeBounds.bounds.extents + Vector3.one, Quaternion.identity, ~Physics.IgnoreRaycastLayer);
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].transform.root != transform && !cols[i].isTrigger)
                {
                    rb.isKinematic = true;
                    return;
                }
            }
            rb.isKinematic = false;
        }
    }
}
