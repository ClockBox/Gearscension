using UnityEngine;
using System.Collections;

public class ClimbState : PlayerState
{
    ClimbingNode currentRight;
    ClimbingNode currentLeft;
    ClimbingNode nextNode;

    ClimbingEdge climbEdge;

    Vector3 Offset;

    const int NONE = -1;
    const int RIGHT = 1;
    const int LEFT = 2;
    int NextMove = LEFT;
    int nodeIndex = NONE;

    bool braced;

    float waitTimer = 0;

    public ClimbState(ClimbingNode node) : base()
    {
        MovementSpeed = 2.5f;
        canClimb = true;
        
        currentRight = node;
        currentLeft = node;
        nextNode = node;

        IK.SetInitialIKPositions(node.rightHand, node.leftHand, node.rightFoot, node.leftFoot);
        if (Manager.HasWeapon(1))
            IK.RightHand.Set(Manager.weapons[1].Grip(1));

        UpdateAnimator();
        anim.SetBool("climbing", true);
        anim.SetFloat("turnAngle", 0);
        rb.velocity = Vector3.zero;
    }
	
	public override CharacterState UpdateState()
    {
        HandleInput();
        return HandleStateChange();
    }

    protected override CharacterState HandleStateChange()
    {
        if (currentLeft == currentRight && climbEdge && Z >= Mathf.Epsilon)
            return new ClimbUpAction(climbEdge);

        if (!currentLeft.Active && !currentRight.Active)
            return new FallState();

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
            return new FallState();
        }
        return null;
    }

    protected override void HandleInput()
    {
        base.HandleInput();
        IK.headWeight = 0;

        X = Input.GetAxisRaw("Horizontal");
        Z = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(X, Z, 0);

        UpdateRoot();
        UpdateAnimator();
        braced = anim.GetBool("braced");

        if (Vector3.Dot(Player.transform.transform.forward, lookDirection) >= 0)
        {
            if (waitTimer >= 1)
            {
                nodeIndex = Mathf.RoundToInt(Mathf.Atan2(moveDirection.x, moveDirection.y) / Mathf.PI * 4);

                if (nodeIndex < 0)
                    nodeIndex += 8;

                if (moveDirection.magnitude > Mathf.Epsilon)
                {
                    waitTimer = 0;
                    FindNextMove();
                }
            }
            waitTimer += MovementSpeed * Time.deltaTime;
            if (waitTimer >= 1)
            {
                if (NextMove == LEFT)
                    currentLeft = nextNode;
                if (NextMove == RIGHT)
                    currentRight = nextNode;
            }
        }
    }

    public void Jump()
    {
        if (braced)
        {
            if (Vector3.Dot(Player.transform.transform.forward, lookDirection) < 0)
            {
                Player.transform.LookAt(Player.transform.position + lookDirection);
                rb.velocity = (lookDirection / 2 + Player.transform.up) * jumpForce;
            }
            else
            {
                canClimb = false;
                if (X > Mathf.Epsilon || X < -Mathf.Epsilon)
                    rb.velocity = (Player.transform.right * X / 2f + Player.transform.up) * jumpForce;
            }
            anim.SetBool("climbing", false);
            anim.SetTrigger("jump");
        }
    }

    public override IEnumerator ExitState()
    {
        anim.SetBool("climbing", false);
        anim.SetFloat("Speed", Z);

        moveDirection = Vector3.zero;
        Player.transform.parent = null;
        
        Player.transform.LookAt(Player.transform.position + Vector3.ProjectOnPlane(Player.transform.forward, Vector3.up));
        yield return null;
    }

    public override void UpdateIK()
    {
        base.UpdateIK();
        
        if (NextMove == RIGHT)
        {
            IKController.LerpIKTarget(IK.RightHand, IKTarget.FromTransform(currentRight.rightHand.transform), IKTarget.FromTransform(nextNode.rightHand.transform), waitTimer);
            if (braced)
                IKController.LerpIKTarget(IK.RightFoot, IKTarget.FromTransform(currentRight.rightFoot.transform), IKTarget.FromTransform(nextNode.rightFoot.transform), waitTimer);
            else
                IKController.LerpIKTarget(IK.RightFoot, IKTarget.FromTransform(currentRight.rightFoot.transform), IKTarget.FromTransform(Player.transform), waitTimer);
        }
        else
        {
            IKController.MoveIKTarget(IK.RightHand, currentRight.rightHand.transform, 10);
            IKController.MoveIKTarget(IK.RightFoot, currentRight.rightFoot.transform, 10);
        }
        
        if (NextMove == LEFT)
        {
            IKController.LerpIKTarget(IK.LeftHand, IKTarget.FromTransform(currentLeft.leftHand.transform), IKTarget.FromTransform(nextNode.leftHand.transform), waitTimer);
            if (braced)
                IKController.LerpIKTarget(IK.LeftFoot, IKTarget.FromTransform(currentLeft.leftFoot.transform), IKTarget.FromTransform(nextNode.leftFoot.transform), waitTimer);
            else
                IKController.LerpIKTarget(IK.LeftFoot, IKTarget.FromTransform(currentLeft.leftFoot.transform), IKTarget.FromTransform(Player.transform), waitTimer);
        }
        else
        {
            IKController.MoveIKTarget(IK.LeftHand, currentLeft.leftHand.transform, 10);
            IKController.MoveIKTarget(IK.LeftFoot, currentLeft.leftFoot.transform, 10);
        }
    }

    private void FindNextMove()
    {
        if (currentRight == currentLeft)
        {
            NextMove = IndexPolarity();
            if (NextMove == RIGHT)
                nextNode = currentRight;
            else if (NextMove == LEFT)
                nextNode = currentLeft;
        }
        else
        {
            if (IndexPolarity() == RIGHT)
            {
                NextMove = LEFT;
                nextNode = currentRight;
            }
            else if(IndexPolarity() == LEFT)
            {
                NextMove = RIGHT;
                nextNode = currentLeft;
            }
            else
            {
                if (nodeIndex == 0)
                {
                    if (currentRight.neighbours[0] == currentLeft)
                    {
                        NextMove = RIGHT;
                        nextNode = currentLeft;
                    }
                    else if (currentLeft.neighbours[0] == currentRight)
                    {
                        NextMove = LEFT;
                        nextNode = currentRight;
                    }
                }
                else if (nodeIndex == 4)
                {
                    if (currentRight.neighbours[4] == currentLeft)
                    {
                        NextMove = RIGHT;
                        nextNode = currentLeft;
                    }
                    else if (currentLeft.neighbours[4] == currentRight)
                    {
                        NextMove = LEFT;
                        nextNode = currentRight;
                    }
                }
            }
        }
        nextNode = calculateNextNode(nextNode);
        if(nextNode)
            Player.transform.parent = nextNode.transform.parent;
    }

    private int IndexPolarity()
    {
        if (nodeIndex == 0 || nodeIndex == 4)
            return NextMove;
        else if (nodeIndex > 4)
            return LEFT;
        else if (nodeIndex < 4 && nodeIndex > 0)
            return RIGHT;
        else return NONE;
    }

    private ClimbingNode calculateNextNode(ClimbingNode currentNode)
    {
        nodeIndex = (nodeIndex + currentNode.Rotation) % 8;
        if (currentRight == currentLeft)
        {
            //Find direct Neighbour from currentNode
            if (!currentNode.neighbours[nodeIndex])
            {
                //No direct Neighbour found searching for indirect Neighbour
                if (currentNode.neighbours[(nodeIndex + 1) % 8])
                    nodeIndex = (nodeIndex + 1) % 8;
                else if (currentNode.neighbours[Mathf.Abs((nodeIndex - 1)) % 8])
                    nodeIndex = Mathf.Abs((nodeIndex - 1)) % 8;
                else return currentNode;
            }
            if (currentNode.neighbours[nodeIndex].GetType() == typeof(ClimbingEdge))
            {
                if (currentNode.neighbours[nodeIndex].Active)
                    climbEdge = currentNode.neighbours[nodeIndex] as ClimbingEdge;
                return currentRight;
            }
            return currentNode.neighbours[nodeIndex] as ClimbingNode;
        }
        return currentNode;
    }

    void UpdateRoot()
    {
        //update Root Position
        Vector3 averagePos = (IK.RightHand.position + IK.RightFoot.position + IK.LeftHand.position + IK.LeftFoot.position) / 4;
        Vector3 averagehandPos = (IK.RightHand.position + IK.LeftHand.position) / 2;

        //float speedOffset = averagePos.magnitude;

        if (Offset == Vector3.zero)
        {
            if (braced)
                Offset = averagePos - (Player.transform.up + Player.transform.forward * 0.4f);
            else
                Offset = averagehandPos - Vector3.up * 1.9f;
        }
        else
        {
            if (braced)
                //Offset = Vector3.MoveTowards(Offset, averagePos - (Player.transform.up * 1.2f + Player.transform.forward * 0.3f), Time.deltaTime * 2 + speedOffset);
                Offset = averagePos - (Player.transform.up * 1.2f + Player.transform.forward * 0.3f);
            else
                //Offset = Vector3.MoveTowards(Offset, averagehandPos - Vector3.up * 1.9f, Time.deltaTime * 2 + speedOffset);
                Offset = averagehandPos - Vector3.up * 1.9f;
        }
        Player.transform.transform.position = Offset;

        //update Root Rotation
        if (braced)
        {
            Quaternion lookPosition = Quaternion.Lerp(currentLeft.leftFoot.transform.rotation, currentRight.rightFoot.transform.rotation, 0.5f);
            Player.transform.rotation = Quaternion.Lerp(Player.transform.rotation, lookPosition, Time.deltaTime * 4);
        }
        else
        {
            Vector3 lookPosition = Player.transform.position + currentLeft.transform.forward + currentRight.transform.forward;
            lookPosition.y = Player.transform.position.y;
            Player.transform.LookAt(lookPosition);
        }
    }

    void UpdateAnimator()
    {
        anim.SetFloat("X",X);
        anim.SetFloat("Z",Z);

        if (nextNode.FreeHang)
            anim.SetBool("braced", false);
        else
            anim.SetBool("braced", Physics.Raycast(((nextNode.leftFoot.transform.position + nextNode.rightFoot.transform.position) / 2) - (nextNode.transform.forward * 0.1f), nextNode.transform.forward, 0.5f));

        if (braced != anim.GetBool("braced"))
            waitTimer = 0;
    }
}
