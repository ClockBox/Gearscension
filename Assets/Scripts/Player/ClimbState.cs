﻿using UnityEngine;
using System.Collections;

public class ClimbState : PlayerState
{
    ClimbingNode[] currentNodes = new ClimbingNode[2];
    ClimbingEdge Edge = null;

    Vector3 playerOffset;
    Vector3 moveDirection;
    Vector3 lookDirection;

    float movementSpeed = 3f;
    float moveX = 0;
    float moveY = 0;

    const int NONE = -1;
    const int RIGHT = 0;
    const int LEFT = 1;

    int nodeIndex;
    int movePolarity = RIGHT;

    float braced;

    public ClimbState(StateManager manager, ClimbingNode node) : base(manager)
    {
        currentNodes[0] = node;
        currentNodes[1] = node;

        braced = node.FreeHang ? 0 : 1;
    }
    public ClimbState(StateManager manager, ClimbingEdge edge) : base(manager)
    {
        ClimbingNode node = edge.neighbours[0] as ClimbingNode;
        Edge = edge;

        currentNodes[0] = node;
        currentNodes[1] = node;

        braced = (node).FreeHang ? 0 : 1;
    }

    //Transitions
    public override IEnumerator EnterState()
    {
        if (Edge)
            yield return ClimbDown(Edge);

        rb.velocity = Vector3.zero;
        anim.SetBool("climbing", true);
        UpdateAnimator();
        
        IK.RightHand.Set(currentNodes[0].rightHand);
        IK.RightFoot.Set(currentNodes[0].rightFoot);
        IK.LeftHand.Set(currentNodes[1].leftHand);
        IK.LeftFoot.Set(currentNodes[1].leftFoot);

        elapsedTime = 0;
        IK.GlobalWeight = 1;

        yield return base.EnterState();
    }
    public override IEnumerator ExitState()
    {
        yield return base.ExitState();

        Player.transform.localEulerAngles = new Vector3(0, Player.transform.localEulerAngles.y, Player.transform.localEulerAngles.z);
        elapsedTime = 0;
        anim.SetBool("climbing", false);

        IK.GlobalWeight = 0;
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (!currentNodes[0].Active && !currentNodes[1].Active)
            stateManager.ChangeState(new UnequipedState(stateManager, false));

        //Jump Input - While Stationary
        else if (Input.GetButtonDown("Jump"))
            Jump();

        else if (Input.GetButtonDown("Equip") || Input.GetButtonDown("Attack"))
            stateManager.ChangeState(new CombatState(stateManager, currentNodes[0]));

        else
        {
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");

            moveDirection = new Vector3(moveX, moveY, 0);

            if (!currentNodes[1].FreeHang && !currentNodes[0].FreeHang && braced == 0)
                yield return BracedTransition(1);

            if (Vector3.Dot(Player.transform.transform.forward, lookDirection) >= 0)
            {
                if (moveDirection.magnitude > Mathf.Epsilon)
                {
                    nodeIndex = Mathf.RoundToInt(Mathf.Atan2(moveDirection.x, moveDirection.y) / Mathf.PI * 4);
                    if (nodeIndex < 0)
                        nodeIndex += 8;

                    movePolarity = FindNextMove();
                    IKPositionNode NextNode = FindNextNode();
                    if (NextNode as ClimbingNode)
                        yield return Climb(NextNode as ClimbingNode);
                    else if (NextNode as ClimbingEdge)
                        yield return ClimbUp(NextNode as ClimbingEdge);
                }
            }
        }
    }
    private int FindNextMove()
    {
        int move = IndexPolarity(nodeIndex);
        if (move == NONE)
        {
            if (currentNodes[1] != currentNodes[0])
                move = (movePolarity + 1) % 2;
            else
                move = movePolarity;
        }
        else if (currentNodes[1] != currentNodes[0])
            move = (move + 1) % 2;
        return move;
    }
    private IKPositionNode FindNextNode()
    {
        IKPositionNode baseNode = currentNodes[movePolarity];
        if (currentNodes[0] != currentNodes[1])
            baseNode = currentNodes[(movePolarity + 1) % 2];

        nodeIndex = (nodeIndex + baseNode.Rotation) % 8;
        if (currentNodes[0] == currentNodes[1])
        {
            //Find direct Neighbour from currentNode
            if (!baseNode.neighbours[nodeIndex])
            {
                //No direct Neighbour found searching for indirect Neighbour
                if (baseNode.neighbours[(nodeIndex + 1) % 8])
                    nodeIndex = (nodeIndex + 1) % 8;
                else if (baseNode.neighbours[Mathf.Abs((nodeIndex - 1)) % 8])
                    nodeIndex = Mathf.Abs((nodeIndex - 1)) % 8;
                else return baseNode;
            }
            return baseNode.neighbours[nodeIndex];
        }
        return baseNode;
    }
    private int IndexPolarity(int index)
    {
        if (index < 4 && index > 0)
            return RIGHT;
        else if (index > 4)
            return LEFT;
        else return NONE;
    }
    private IEnumerator BracedTransition(float end)
    {
        float start = braced;
        elapsedTime = 0;

        while (elapsedTime <= 1.1f)
        {
            braced = Mathf.Lerp(start, end, elapsedTime);
            UpdateMovement();
            UpdateIK();
            UpdateAnimator();

            elapsedTime += Time.deltaTime * 3;
            yield return null;
        }
    }

    //Actions
    private void Jump()
    {
        if (braced == 1)
        {
            anim.SetBool("climbing", false);

            if (Vector3.Dot(Player.transform.transform.forward, lookDirection) < 0)
            {
                Player.transform.LookAt(Player.transform.position + lookDirection);
                rb.velocity = (lookDirection.normalized / 2 + Player.transform.up) * 5;
            }
            else
                rb.velocity = (Player.transform.right * moveX / 2 + Vector3.up * moveY) * 5 + Vector3.up;
        }
        Player.transform.localEulerAngles = new Vector3(0, Player.transform.localEulerAngles.y, Player.transform.localEulerAngles.z);
        canClimb = false;
        stateManager.ChangeState(new UnequipedState(stateManager, false));
    }

    private IEnumerator Climb(ClimbingNode nextNode)
    {
        UpdateAnimator();

        if (nextNode.FreeHang && braced != 0)
             yield return BracedTransition(0);

        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
                yield break;
            }
            //Move Right Side
            if (movePolarity == RIGHT)
            {
                IKController.LerpIKTarget(IK.RightHand, IKTarget.FromTransform(currentNodes[RIGHT].rightHand.transform), IKTarget.FromTransform(nextNode.rightHand.transform), elapsedTime);
                IKController.LerpIKTarget(IK.RightFoot, IKTarget.FromTransform(currentNodes[RIGHT].rightFoot.transform), IKTarget.FromTransform(nextNode.rightFoot.transform), elapsedTime);
            }
            else
            {
                IK.RightHand.Set(currentNodes[RIGHT].rightHand);
                IK.RightFoot.Set(currentNodes[RIGHT].rightFoot);
            }

            //Move Left Side
            if (movePolarity == LEFT)
            {
                IKController.LerpIKTarget(IK.LeftHand, IKTarget.FromTransform(currentNodes[LEFT].leftHand.transform), IKTarget.FromTransform(nextNode.leftHand.transform), elapsedTime);
                IKController.LerpIKTarget(IK.LeftFoot, IKTarget.FromTransform(currentNodes[LEFT].leftFoot.transform), IKTarget.FromTransform(nextNode.leftFoot.transform), elapsedTime);
            }
            else
            {
                IK.LeftHand.Set(currentNodes[LEFT].leftHand);
                IK.LeftFoot.Set(currentNodes[LEFT].leftFoot);
            }

            //Root Rotaion - While moving
            Player.transform.rotation = Quaternion.Lerp(
                Quaternion.Lerp(currentNodes[LEFT].transform.rotation, currentNodes[RIGHT].transform.rotation, 0.5f),
                Quaternion.Lerp(currentNodes[(movePolarity + 1) % 2].transform.rotation, nextNode.transform.rotation, 0.5f),
                elapsedTime);
            if(braced < 0.5f)
                Player.transform.localEulerAngles = new Vector3(0, Player.transform.localEulerAngles.y, Player.transform.localEulerAngles.z);

            //Root Position - While moving
            Player.transform.position = Vector3.Lerp(
                (currentNodes[LEFT].PlayerPosition + currentNodes[RIGHT].PlayerPosition) / 2, 
                (currentNodes[(movePolarity + 1) % 2].PlayerPosition + nextNode.PlayerPosition) / 2,
                elapsedTime);

            elapsedTime += Time.deltaTime * movementSpeed;

            //Jump Input - While Moving
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
                elapsedTime = 1;
            }
            yield return null;
        }
        currentNodes[movePolarity] = nextNode;
    }
    private IEnumerator ClimbDown(ClimbingEdge EdgeNode)
    {
        float braced;
        float waitTime;

        if (currentNodes[0].FreeHang)
        {
            waitTime = 2.0f;
            braced = 0;
        }
        else
        {
            waitTime = 1.3f;
            braced = 1;
        }
        anim.SetFloat("braced", braced);

        Collider col = Player.GetComponent<Collider>();
        col.enabled = false;

        anim.SetTrigger("climbDown");
        IK.headWeight = 0;

        rb.velocity = Vector3.zero;

        Vector3 Offset = (currentNodes[1].transform.position + currentNodes[0].transform.position) / 2;
        if (braced == 1)
            Offset += -(currentNodes[1].transform.forward + currentNodes[0].transform.forward).normalized * 0.4f - Player.transform.up * 1.5f;
        else
            Offset += -Player.transform.up * 2f;

        Quaternion startRotation = Player.transform.rotation;
        Vector3 startPosition = Player.transform.position;

        elapsedTime = 0;
        while (elapsedTime <= 1)
        {
            //IK
            IK.SetIKPositions(currentNodes[0].rightHand, currentNodes[0].leftHand, currentNodes[0].rightFoot, currentNodes[0].leftFoot);
            IK.RightHand.weight = Mathf.Lerp(0, 1, elapsedTime);
            IK.LeftHand.weight = Mathf.Lerp(0, 1, elapsedTime);
            if (braced == 1)
            {
                IK.RightFoot.weight = Mathf.Lerp(0, 1, elapsedTime);
                IK.LeftFoot.weight = Mathf.Lerp(0, 1, elapsedTime);
            }

            //rotation
            Player.transform.rotation = Quaternion.Lerp(startRotation, currentNodes[0].transform.rotation, elapsedTime * 3);

            //position
            Vector3 lerp1 = Vector3.Lerp(startPosition, Edge.transform.position, elapsedTime);
            Vector3 lerp2 = Vector3.Lerp(Edge.transform.position, Offset, elapsedTime * 1.1f);
            Player.transform.position = Vector3.Lerp(lerp1, lerp2, elapsedTime);

            elapsedTime += Time.deltaTime/waitTime;
            yield return null;
        }
        col.enabled = true;
    }
    private IEnumerator ClimbUp(ClimbingEdge EdgeNode)
    {
        float waitTime;
        if (braced == 1)
            waitTime = 1.0f;
        else if (braced == 0)
            waitTime = 3.5f;
        else
            yield break;

        Collider col = Player.GetComponent<Collider>();
        col.enabled = false;

        anim.SetTrigger("climbUp");
        IK.headWeight = 0;
        IK.RightFoot.weight = 0;
        IK.LeftFoot.weight = 0;

        rb.velocity = Vector3.zero;

        elapsedTime = 0;
        while (elapsedTime <= waitTime)
        {
            IK.RightHand.weight = Mathf.Lerp(1, 0, elapsedTime);
            IK.LeftHand.weight = Mathf.Lerp(1, 0, elapsedTime);

            Player.transform.position += anim.velocity * Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Player.transform.LookAt(Player.transform.position + Vector3.ProjectOnPlane(Player.transform.forward, Vector3.up));
        col.enabled = true;
        stateManager.ChangeState(new UnequipedState(stateManager, true));
    }

    //State Updates
    protected override void UpdateMovement()
    {
        lookDirection = Camera.main.transform.forward;
        lookDirection = Vector3.ProjectOnPlane(lookDirection, Player.transform.up);

        //Root Position - While Stationary
        Player.transform.position = (currentNodes[1].PlayerPosition + currentNodes[0].PlayerPosition) / 2;

        //Root Rotation - While Stationary
        Player.transform.rotation = Quaternion.Lerp(currentNodes[1].transform.rotation, currentNodes[0].transform.rotation, 0.5f);
        if (braced < 0.5f)
            Player.transform.localEulerAngles = new Vector3(0, Player.transform.localEulerAngles.y, Player.transform.localEulerAngles.z);
    }
    protected override void UpdateIK()
    {
        IK.SetIKPositions(currentNodes[0].rightHand, currentNodes[1].leftHand, currentNodes[0].rightFoot, currentNodes[1].leftFoot);

        IK.GlobalWeight = 1;
        IK.RightFoot.weight = Mathf.Pow(braced,4);
        IK.LeftFoot.weight = Mathf.Pow(braced,4);
        IK.headWeight = 0;
    }
    protected override void UpdateAnimator()
    {
        anim.SetFloat("braced", braced);
    }
}