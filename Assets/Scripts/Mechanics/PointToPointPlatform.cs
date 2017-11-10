using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToPointPlatform : Platform
{
    [SerializeField]
    private Transform[] nodes;

    private int currentMoveNode;
    private int nextMoveNode;

    private int currentRotationNode;
    private int nextRotationNode;

    private float elapsedMoveTime;
    private float elapsedRotationTime;

    [SerializeField]
    private bool loopMovement;
    public bool LoopMovement
    {
        get { return loopMovement; }
        set { loopMovement = value; }
    }
    [SerializeField]
    private bool loopRotation;
    public bool LoopRotation
    {
        get { return loopRotation; }
        set { loopRotation = value; }
    }

    [SerializeField]
    private bool move;
    public bool Move
    {
        get { return move; }
        set { move = value; }
    }

    [SerializeField]
    private bool rotate;
    public bool Rotate
    {
        get { return rotate; }
        set { rotate = value; }
    }

    public bool MoveAndRotate
    {
        set
        {
            move = value;
            rotate = value;
        }
    }

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    [SerializeField]
    private float rotationSpeed;
    public float RotationSpeed
    {
        get { return rotationSpeed; }
        set { rotationSpeed = value; }
    }

    private void Start()
    {
        Reset();
    }

    private void Update()
    {
        if (move)
        {
            transform.position = Vector3.Lerp(nodes[currentMoveNode].position, nodes[nextMoveNode].position, elapsedMoveTime);
            if (elapsedMoveTime >= 1)
            {
                currentMoveNode = (currentMoveNode + 1) % nodes.Length;
                nextMoveNode = (currentMoveNode + 1) % nodes.Length;

                if (!loopMovement)
                    MoveAndRotate = false;
                elapsedMoveTime = 0;
            }
            else elapsedMoveTime += Time.deltaTime / moveSpeed;
        }
        if (rotate)
        {
            transform.rotation = Quaternion.Lerp(nodes[currentRotationNode].rotation, nodes[nextRotationNode].rotation, elapsedRotationTime);
            if (elapsedRotationTime >= 1)
            {
                currentRotationNode = (currentRotationNode + 1) % nodes.Length;
                nextRotationNode = (currentRotationNode + 1) % nodes.Length;

                if (!loopRotation)
                    MoveAndRotate = false;
                elapsedRotationTime = 0;
            }
            else elapsedRotationTime += Time.deltaTime / rotationSpeed;
        }
    }

    private void Reset()
    {
        currentMoveNode = 0;
        nextMoveNode = 1;

        currentRotationNode = 0;
        nextRotationNode = 1;

        transform.position = nodes[currentMoveNode].position;
        transform.rotation = nodes[currentRotationNode].rotation;
    }
}
