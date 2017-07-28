using UnityEngine;
using System.Collections;

public class PushState : PlayerState
{
    Rigidbody pushObject;

    Vector3 desiredDirection;
    Vector3 lookDirection;
    Vector3 moveDirection;

    Vector3 ReferencePoint;
    Vector3 PlayerOffset;
    Vector3 ObjectOffset;

    Vector3 RightHandOffset;
    Vector3 LeftHandOffset;

    float movementSpeed = 5;
    float moveX = 0;
    float moveY = 0;

    public PushState(StateManager manager, GameObject pushObject) : base(manager)
    {
        this.pushObject = pushObject.GetComponent<Rigidbody>();
    }
    
    //Transitions
    public override IEnumerator EnterState()
    {
        anim.SetBool("pushing", true);

        elapsedTime = 0;
        rb.velocity = Vector3.zero;
        pushObject.isKinematic = false;
        FindHandPositions();

        while (elapsedTime <= 1)
        {
            IK.RightHand.weight = Mathf.Lerp(0, 1, elapsedTime);
            IK.LeftHand.weight = Mathf.Lerp(0, 1, elapsedTime);
            elapsedTime += Time.deltaTime * 3;
            yield return null;
        }
        yield return base.EnterState();
    }
    public override IEnumerator ExitState()
    {
        yield return base.ExitState();
        anim.SetBool("pushing", false);

        elapsedTime = 0;
        pushObject.isKinematic = true;

        while (elapsedTime <= 1)
        {
            IK.RightHand.weight = Mathf.Lerp(1, 0, elapsedTime);
            IK.LeftHand.weight = Mathf.Lerp(1, 0, elapsedTime);
            elapsedTime += Time.deltaTime * 3;
            yield return null;
        }
        IK.GlobalWeight = 0;
    }

    //Actions
    IEnumerator PullObject()
    {
        elapsedTime = 0;
        Vector3 startPos = pushObject.transform.position;
        Vector3 endPos = Player.transform.position - ObjectOffset;

        RaycastHit hit;
        if (pushObject.SweepTest(endPos - startPos, out hit, (endPos - startPos).magnitude))
        {
            if (hit.collider.gameObject != Player.gameObject)
            {
                yield return ResetPlayer();
                yield break;
            }
        }
        while (elapsedTime <= 0.5f)
        {
            pushObject.position = Vector3.Lerp(startPos, endPos, elapsedTime * 2);
            UpdateIK();
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator ResetPlayer()
    {
        elapsedTime = 0;
        Vector3 startPos = Player.transform.position;
        Vector3 endPos = pushObject.transform.position + ObjectOffset;
        while (elapsedTime <= 0.5f)
        {
            rb.position = Vector3.Lerp(startPos, endPos, elapsedTime * 2);
            UpdateIK();
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (Input.GetButtonDown("Action"))
            stateManager.ChangeState(new UnequipedState(stateManager, true));

        else if (pushObject.velocity.y < -1)
            stateManager.ChangeState(new UnequipedState(stateManager, true));

        else if (PlayerOffset.magnitude > 0.5f)
        {
            moveDirection = Vector3.zero;
            yield return PullObject();
        }

        yield break;
    }
    void FindHandPositions()
    {
        //RayCast- Right Hand
        Vector3 rightRayStart = Player.transform.up * 1.4f + Player.transform.right * 0.5f;
        rightRayStart.y = Mathf.Clamp(rightRayStart.y, 0.5f, (pushObject.transform.position.y - Player.transform.position.y) + pushObject.transform.localScale.y / 2);
        Vector3 rightRayDirection = Player.transform.forward - Player.transform.right * 0.5f;
        RaycastHit rightHit;

        if (Physics.Raycast(Player.transform.position + rightRayStart, rightRayDirection, out rightHit, 1f))
        {
            IK.RightHand.position = (rightHit.point - Player.transform.up * 0.1f - Player.transform.forward * 0.1f);
            IK.RightHand.rotation = Quaternion.FromToRotation(Player.transform.up, rightHit.normal) * Player.transform.rotation;
            IK.RightHand.weight = 1f;
        }
        else IK.RightHand.weight = 0f;

        //RayCast- left Hand
        Vector3 leftRayStart = Player.transform.up * 1.4f - Player.transform.right * 0.5f;
        leftRayStart.y = Mathf.Clamp(leftRayStart.y, 1, (pushObject.transform.position.y - Player.transform.position.y) + pushObject.transform.localScale.y / 2);
        Vector3 leftRayDirection = Player.transform.forward + Player.transform.right * 0.5f;
        RaycastHit leftHit;

        if (Physics.Raycast(Player.transform.position + leftRayStart, leftRayDirection, out leftHit, 1f))
        {
            IK.LeftHand.position = (leftHit.point - Player.transform.up * 0.1f - Player.transform.forward * 0.1f);
            IK.LeftHand.rotation = Quaternion.FromToRotation(Player.transform.up, leftHit.normal) * Player.transform.rotation;
            IK.LeftHand.weight = 1f;
        }
        else IK.LeftHand.weight = 0f;

        RightHandOffset = IK.RightHand.position - pushObject.transform.position;
        LeftHandOffset = IK.LeftHand.position - pushObject.transform.position;

        ObjectOffset = Player.transform.position - pushObject.transform.position;

        ReferencePoint = pushObject.transform.position + (RightHandOffset + LeftHandOffset) / 2;
        ReferencePoint.y = Player.transform.position.y;
    }

    //State Updates
    protected override void UpdateMovement()
    {
        if (pushObject)
        {
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");

            if (Mathf.Abs(moveY) >= Mathf.Abs(moveX))
                moveX = 0;
            else moveY = 0;

            movementSpeed = 2;

            lookDirection = Camera.main.transform.forward;
            lookDirection = Vector3.ProjectOnPlane(lookDirection, Player.transform.up);

            desiredDirection = Quaternion.FromToRotation(Player.transform.forward, lookDirection) * (Player.transform.right * moveX + Player.transform.forward * moveY);
            moveDirection = Vector3.MoveTowards(moveDirection, desiredDirection * movementSpeed, 10 * Time.deltaTime);

            if (desiredDirection.magnitude > 0)
                moveDirection = Vector3.RotateTowards(moveDirection, desiredDirection + lookDirection * 0.01f, 20 * Time.deltaTime, 0);

            if (moveDirection.magnitude > movementSpeed)
                moveDirection = moveDirection.normalized * movementSpeed;

            ReferencePoint = pushObject.transform.position + (RightHandOffset + LeftHandOffset) / 2;
            ReferencePoint.y = Player.transform.position.y;

            PlayerOffset = Player.transform.position - ReferencePoint;
            PlayerOffset.y = 0;
        }
    }
    protected override void UpdateAnimator()
    {
        Vector3 speed = rb.velocity;
        speed.y = 0;
        anim.SetFloat("Speed", speed.magnitude);
    }
    protected override void UpdateIK()
    {
        if (pushObject)
        {
            IK.RightHand.position = pushObject.transform.transform.position + RightHandOffset;
            IK.LeftHand.position = pushObject.transform.position + LeftHandOffset;

            IK.RightFoot.weight = anim.GetFloat("RightFootWeight");
            IK.LeftFoot.weight = anim.GetFloat("LeftFootWeight");
        }
    }
    protected override void UpdatePhysics()
    {
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        rb.AddForce(Player.transform.up * -20f * rb.mass);

        //Foot Raycasts
        RaycastHit RightHit;
        Transform RightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot);
        if (Physics.Raycast(RightFoot.position + Player.transform.up * 0.1f, -Player.transform.up, out RightHit, 1f))
        {
            IK.RightFoot.position = RightHit.point + Player.transform.up * 0.12f;
            IK.RightFoot.rotation = Quaternion.FromToRotation(Player.transform.up, RightHit.normal) * Player.transform.rotation;
        }
        else IK.RightFoot.weight = 0;

        RaycastHit LeftHit;
        Transform LeftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        if (Physics.Raycast(LeftFoot.position + Player.transform.up * 0.1f, -Player.transform.up, out LeftHit, 1f))
        {
            IK.LeftFoot.position = LeftHit.point + Player.transform.up * 0.12f;
            IK.LeftFoot.rotation = Quaternion.FromToRotation(Player.transform.up, LeftHit.normal) * Player.transform.rotation;
        }
        else IK.LeftFoot.weight = 0;
    }
}