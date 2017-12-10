using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCube : MonoBehaviour
{
    public bool climbable;
    public bool pushable;
    public GameObject climbNodePrefab;
    public GameObject pushNodePrefab;

    public Collider[] movementColliders;
    public Collider[] staticColliders;

    private bool grounded;
    private List<IKPositionNode> ikNodes;

    [SerializeField]
    private bool pushing;
    public bool Pushing
    {
        get { return pushing; }
        set
        {
            pushing = value;
            ToggleColliders();
        }
    }

    private Collider cubeBounds;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start ()
    {
        ikNodes = new List<IKPositionNode>();
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
                    ikNodes.Add(node);
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

    private void ToggleColliders()
    {
        for (int i = 0; i < movementColliders.Length; i++)
            movementColliders[i].enabled = pushing;

        for (int i = 0; i < staticColliders.Length; i++)
            staticColliders[i].enabled = !pushing;
    }

    private void UpdateGrounded()
    {
        grounded = !rb.isKinematic;
        for(int i= 0;i<ikNodes.Count;i++)
        {
            ikNodes[i].enabled = grounded;
        }
    }
    
    private void FixedUpdate()
    {
        if (pushing)
        {
            rb.isKinematic = false;
        }
        else
        {
            Collider[] cols = Physics.OverlapBox(transform.position, cubeBounds.bounds.extents - new Vector3(0.25f, 0, 0.25f), Quaternion.identity, ~6);
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

        if (grounded == rb.isKinematic)
            for (int i = 0; i < ikNodes.Count; i++)
            {
                UpdateGrounded();
            }
    }
}
