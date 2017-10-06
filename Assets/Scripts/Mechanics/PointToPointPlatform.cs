using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToPointPlatform : Platform
{
    public Transform[] movementNodes;
    Vector3 startPos;
    Vector3 endPos;
    int currentNode;

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
    }

    private void Update()
    {
        if (move)
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

        if(reset)
        {
            transform.position = startPos;
            currentNode = 0;
        }
    }
}
