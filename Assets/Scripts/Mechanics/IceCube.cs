using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCube : MonoBehaviour
{
    public bool climbable;
    public bool pushable;

    public GameObject climbNodePrefab;
    public GameObject pushNodePrefab;

    private Collider cubeBounds;

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
                    Instantiate(climbNodePrefab,
                        transform.position + rotation * new Vector3(extents.x, extents.y, i),
                        rotation * Quaternion.Euler(0, -90, 0),
                        transform);
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
}
