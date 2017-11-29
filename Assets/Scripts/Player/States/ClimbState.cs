using UnityEngine;
using System.Collections;

public class ClimbState : PlayerState
{
    protected ClimbingNode[] currentNodes = new ClimbingNode[2];
    protected ClimbingEdge Edge = null;
    
    protected Vector3 moveDirection;
    protected Vector3 lookDirection;

    protected float movementSpeed = 3f;
    protected float moveX = 0;
    protected float moveY = 0;

    private const int NONE = -1;
    private const int RIGHT = 0;
    private const int LEFT = 1;

    private int nodeIndex;
    private int movePolarity = RIGHT;

    private float braced;

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
    public override IEnumerator EnterState(PlayerState prevState)
    {
        if ((prevState as ClimbState) == null)
        {
            if (Edge)
                yield return ClimbDown(Edge);

            rb.velocity = Vector3.zero;
            anim.SetBool("climbing", true);
            UpdateAnimator();

            if (!Edge)
            {
                elapsedTime = 0;
                while (elapsedTime <= 1)
                {
                    Player.transform.position = Vector3.MoveTowards(Player.transform.position, currentNodes[0].PlayerPosition, 4 * Time.deltaTime);

                    IK.SetIKPositions(currentNodes[0].rightHand, currentNodes[1].leftHand, currentNodes[0].rightFoot, currentNodes[1].leftFoot);

                    IK.RightHand.weight = elapsedTime;
                    IK.LeftHand.weight = elapsedTime;
                    IK.RightFoot.weight = elapsedTime * braced;
                    IK.LeftFoot.weight = elapsedTime * braced;


                    elapsedTime += Time.deltaTime * 5;
                    yield return null;
                }
            }
        }
        else UpdateIK();

        UpdateMovement();
        yield return base.EnterState(prevState);
    }
    public override IEnumerator ExitState(PlayerState nextState)
    {
        yield return base.ExitState(nextState);

        if ((nextState as ClimbState) == null)
        {
            Player.transform.localEulerAngles = new Vector3(0, Player.transform.localEulerAngles.y, Player.transform.localEulerAngles.z);
            elapsedTime = 0;
            anim.SetBool("climbing", false);
            
            elapsedTime = 1;
            while (elapsedTime >= 0)
            {
                elapsedTime -= Time.deltaTime * 8;

                IK.RightHand.weight = elapsedTime;
                IK.LeftHand.weight = elapsedTime;
                IK.RightFoot.weight = elapsedTime * braced;
                IK.LeftFoot.weight = elapsedTime * braced;

                yield return null;
            }
        }
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (!currentNodes[0].Active && !currentNodes[1].Active)
            stateManager.ChangeState(new UnequipedState(stateManager, false));

        //Jump Input - While Stationary
        else if (Input.GetButtonDown("Jump"))
            Jump();

        else if (currentNodes[0] == currentNodes[1] && (Input.GetButtonDown("Equip") || Input.GetButtonDown("Attack") || Player.RightTrigger.Down))
            stateManager.ChangeState(new HookState(stateManager, currentNodes[0]));

        else if (!currentNodes[0] && !currentNodes[1])
            stateManager.ChangeState(new UnequipedState(stateManager, false));

        else
        {
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");

            moveDirection = new Vector3(moveX, moveY, 0);

            if (!currentNodes[1].FreeHang && !currentNodes[0].FreeHang && braced == 0)
                yield return BracedTransition(1);

            if (Vector3.Dot(Vector3.Project(Player.transform.forward, Vector3.up), lookDirection) >= 0)
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

    //State Actions
    private void Jump()
    {
        if (braced == 1)
        {
            anim.SetBool("climbing", false);
            anim.SetBool("isGrounded", false);

            //Wall Eject
            if (Vector3.Dot(currentNodes[0].transform.transform.forward, lookDirection) < 0)
            {
                Player.transform.LookAt(Player.transform.position + lookDirection);
                rb.velocity = Vector3.ProjectOnPlane(lookDirection.normalized, Vector3.up) * 2.5f + Player.transform.up * 2;
            }
            //Drop/Move allong wall
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
        while (elapsedTime <= 1)
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
        IK.HeadWeight = 0;

        rb.velocity = Vector3.zero;

        Quaternion startRotation = Player.transform.rotation;
        Vector3 startpos = Player.transform.position;
        Vector3 endPos = currentNodes[0].PlayerPosition;

        elapsedTime = 0;
        while (elapsedTime <= 1)
        {
            Vector3 offset = currentNodes[0].PlayerPosition - endPos;

            // IK
            IK.SetIKPositions(currentNodes[0].rightHand, currentNodes[0].leftHand, currentNodes[0].rightFoot, currentNodes[0].leftFoot);
            IK.RightHand.weight = elapsedTime;
            IK.LeftHand.weight = elapsedTime;
            if (braced == 1)    
            {
                IK.RightFoot.weight = elapsedTime;
                IK.LeftFoot.weight = elapsedTime;
            }

            // Rotation
            Player.transform.rotation = Quaternion.Lerp(startRotation, currentNodes[0].transform.rotation, elapsedTime * 3);

            // Position
            Vector3 lerp1 = Vector3.Lerp(startpos + offset, EdgeNode.transform.position, elapsedTime * 1.1f);
            Vector3 lerp2 = Vector3.Lerp(EdgeNode.transform.position, endPos + offset, elapsedTime);
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
        IK.HeadWeight = 0;
        IK.RightFoot.weight = 0;
        IK.LeftFoot.weight = 0;

        Vector3 startpos = currentNodes[0].PlayerPosition;
        Vector3 endPos = EdgeNode.transform.position + EdgeNode.transform.forward * 0.5f + Vector3.down * 0.2f;
        
        rb.velocity = Vector3.zero;

        elapsedTime = 0;
        while (elapsedTime <= waitTime)
        {
            Vector3 offset = currentNodes[0].PlayerPosition - startpos;

            // IK
            IK.SetIKPositions(currentNodes[0].rightHand, currentNodes[0].leftHand, currentNodes[0].rightFoot, currentNodes[0].leftFoot);

            IK.RightHand.weight = Mathf.Lerp(1, 0, elapsedTime);
            IK.LeftHand.weight = Mathf.Lerp(1, 0, elapsedTime);

            // Position
            Vector3 lerp1 = Vector3.Lerp(startpos + offset, EdgeNode.transform.position, elapsedTime * 1.1f);
            Vector3 lerp2 = Vector3.Lerp(EdgeNode.transform.position, endPos + offset, elapsedTime);
            Player.transform.position = Vector3.Lerp(lerp1, lerp2, elapsedTime);

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
        if (currentNodes[0] && currentNodes[1])
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
    }
    protected override void UpdateIK()
    {
        IK.SetIKPositions(currentNodes[0].rightHand, currentNodes[1].leftHand, currentNodes[0].rightFoot, currentNodes[1].leftFoot);

        IK.GlobalWeight = 1;
        IK.RightFoot.weight = Mathf.Pow(braced,4);
        IK.LeftFoot.weight = Mathf.Pow(braced,4);
        IK.HeadWeight = Mathf.Lerp(IK.HeadWeight, 0, 0.2f);
    }
    protected override void UpdateAnimator()
    {
        anim.SetFloat("braced", braced);
    }
}