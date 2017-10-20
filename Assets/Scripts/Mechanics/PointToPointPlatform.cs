using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToPointPlatform : Platform
{
    /// 
    /// 
    /// 
    /// When using be sure to add a point to point node at start location when looping (temporary?)
    /// 
    /// 
    /// 
    
    public Transform[] movementNodes;
    private Vector3 startPos;
    private Vector3 endPos;
    private int currentNode;

    [SerializeField]
    private bool loop;
    public bool Loop
    {
        get { return loop; }
        set { loop = value; }
    }

    [SerializeField]
    private bool move;
    public bool Move
    {
        get { return move; }
        set { move = value; }
    }

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    private void Start()
    {
        startPos = transform.position;
        currentNode = 0;
    }

    private void Update()
    {
        if (move)
        {
            transform.Translate(endPos.normalized * Time.deltaTime * moveSpeed);
            endPos = movementNodes[currentNode].position - transform.position;
            if (endPos.magnitude <= 0.2)
            {
                currentNode = (currentNode + 1) % movementNodes.Length;
                if (!Loop)
                    move = false;
            }
        }
    }

    private void Reset()
    {
        transform.position = startPos;
        currentNode = 0;
    }
}
