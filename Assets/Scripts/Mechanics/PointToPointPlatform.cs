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

    private bool reset;
    public bool Reset
    {
        get { return reset; }
        set { reset = value; }
    }

    private void Start()
    {
        startPos = transform.position;
        move = true;
        endPos = movementNodes[currentNode].position - transform.position;
    }

    private void Update()
    {
        if (move && loop && movementNodes.Length > 1)
        {
            if (endPos.magnitude <= 0.1)
            {
                if (currentNode >= movementNodes.Length - 1)
                {
                    currentNode = 0;
                }
                else
                    currentNode++;

                endPos = movementNodes[currentNode].position - transform.position;
            }

            transform.Translate(endPos.normalized * Time.deltaTime * moveSpeed);
            endPos = movementNodes[currentNode].transform.position - transform.position;
        }
        else if (move || movementNodes.Length <= 1)
        {
            if(endPos.magnitude > 0.1)
            {
                endPos = movementNodes[currentNode].position - transform.position;
                transform.Translate(endPos.normalized * Time.deltaTime * moveSpeed);
            }
        }

        if(reset)
        {
            transform.position = startPos;
            currentNode = 0;
        }
    }
}
