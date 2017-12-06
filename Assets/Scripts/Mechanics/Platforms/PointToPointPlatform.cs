using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToPointPlatform : Platform
{
    #region Variables
    private Animator anim;
    
    [SerializeField]
    private Transform[] nodes;
    [Space(10)]

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
        set
        {
            move = value;
            active = move || rotate;
        }
    }

    [SerializeField]
    private bool rotate;
    public bool Rotate
    {
        get { return rotate; }
        set
        {
            rotate = value;
            active = move || rotate;
        }
    }

    public bool MoveAndRotate
    {
        set
        {
            move = value;
            rotate = value;
            active = move || rotate;
        }
    }

    [SerializeField]
    private float moveTime;
    public float MoveTime
    {
        get { return moveTime; }
        set { moveTime = value; }
    }
    [SerializeField]
    private float rotationTime;
    public float RotationTime
    {
        get { return rotationTime; }
        set { rotationTime = value; }
    }
    #endregion

    public override void Activate()
    {
        base.Activate();
        if (anim) anim.enabled = true;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        if (anim) anim.enabled = false;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentMoveNode = 0;
        nextMoveNode = 1;

        currentRotationNode = 0;
        nextRotationNode = 1;

        transform.position = nodes[currentMoveNode].position;
        transform.rotation = nodes[currentRotationNode].rotation;
    }

    private void FixedUpdate()
    {
        // Movement
        if (move)
            elapsedMoveTime += Time.deltaTime / moveTime;
        else elapsedMoveTime -= Time.deltaTime / moveTime;
        if (elapsedMoveTime >= 1)
        {
            currentMoveNode = (currentMoveNode + 1) % nodes.Length;
            nextMoveNode = (currentMoveNode + 1) % nodes.Length;

            if (!loopMovement)
                Move = false;
            elapsedMoveTime = 0;
        }
        else if (elapsedMoveTime > 0)
            transform.position = Vector3.Lerp(nodes[currentMoveNode].position, nodes[nextMoveNode].position, elapsedMoveTime);
        else if(elapsedMoveTime < 0)
            elapsedMoveTime = 0;

        // Rotation
        if (rotate)
            elapsedRotationTime += Time.deltaTime / rotationTime;
        else elapsedRotationTime -= Time.deltaTime / rotationTime;
        if (elapsedRotationTime >= 1)
        {
            currentRotationNode = (currentRotationNode + 1) % nodes.Length;
            nextRotationNode = (currentRotationNode + 1) % nodes.Length;

            if (!loopRotation)
                Rotate = false;
            elapsedRotationTime = 0;
        }
        else if(elapsedRotationTime > 0)
            transform.rotation = Quaternion.Lerp(nodes[currentRotationNode].rotation, nodes[nextRotationNode].rotation, elapsedRotationTime);
        else if (elapsedRotationTime < 0)
            elapsedRotationTime = 0;
    }

    public void MoveTo(int nodeIndex)
    {
        if (currentMoveNode == nodeIndex)
            Move = false;
        else
        {
            nextMoveNode = (nodeIndex) % nodes.Length;
            Move = true;
        }
    }

    public void RotateTo(int nodeIndex)
    {
        if (currentRotationNode == nodeIndex)
            Rotate = false;
        else
        {
            nextRotationNode = (nodeIndex) % nodes.Length;
            Rotate = true;
        }
    }

    public void Reset()
    {
        currentMoveNode = 0;
        nextMoveNode = 1;

        currentRotationNode = 0;
        nextRotationNode = 1;

        Move = false;
        Rotate = false;

        transform.position = nodes[currentMoveNode].position;
        transform.rotation = nodes[currentRotationNode].rotation;
    }
}
