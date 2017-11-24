using UnityEngine;
using System.Collections;

public class PushState : PlayerState
{
    HandNode pushNode;
    Rigidbody pushObject;

    Vector3 desiredDirection;
    Vector3 lookDirection;
    Vector3 moveDirection;

    Vector3 RightHandOffset;
    Vector3 LeftHandOffset;

    float movementSpeed = 5;
    float moveX = 0;
    float moveY = 0;

    public PushState(StateManager manager, HandNode pushNode) : base(manager)
    {
        this.pushNode = pushNode;
        this.pushObject = pushNode.rb;
    }
    
    //Transitions
    public override IEnumerator EnterState(PlayerState prevState)
    {
        anim.SetBool("pushing", true);

        rb.velocity = Vector3.zero;
        lookDirection = pushNode.transform.forward;
        Player.transform.LookAt(Player.transform.position + lookDirection);
        pushObject.isKinematic = false;

        IK.RightHand.Set(pushNode.rightHand);
        IK.LeftHand.Set(pushNode.leftHand);

        elapsedTime = 0;
        while (elapsedTime <= 1)
        {
            IK.RightHand.weight = Mathf.Lerp(0, 1, elapsedTime);
            IK.LeftHand.weight = Mathf.Lerp(0, 1, elapsedTime);
            elapsedTime += Time.deltaTime * 3;
            yield return null;
        }
        yield return base.EnterState(prevState);
    }
    public override IEnumerator ExitState(PlayerState nextState)
    {
        yield return base.ExitState(nextState);
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

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (Input.GetButtonDown("Action"))
            stateManager.ChangeState(new UnequipedState(stateManager, true));

        else if (!grounded)
            stateManager.ChangeState(new UnequipedState(stateManager, false));

        else if (!pushObject || pushObject.velocity.y < -0.5f)
            stateManager.ChangeState(new UnequipedState(stateManager, true));

        else if (Player.transform.InverseTransformDirection(rb.velocity).z < -0.1f)
            yield return MoveBack();

        yield break;
    }

    private IEnumerator MoveBack()
    {
        Vector3 startPos = pushObject.position;
        moveDirection = -Player.transform.forward * 1;
        anim.SetFloat("Z", -2);
        float distance = 1f;
        float time = 1f;
        float Pi = 3.14159f;
        elapsedTime = 0;
        while (elapsedTime < time && Player.transform.InverseTransformDirection(rb.velocity).z < -0.1f)
        {
            Debug.Log(Player.transform.InverseTransformDirection(rb.velocity).z);
            UpdateIK();
            float moveForce = -(distance / 2 * Mathf.Cos(Pi * elapsedTime / time)) + distance / 2;
            pushObject.position = startPos + -Player.transform.forward * moveForce;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        pushObject.velocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        moveDirection = Vector3.zero;
    }

    //State Updates
    protected override void UpdateMovement()
    {
        if (pushObject)
        {
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");
            movementSpeed = 2;

            lookDirection = Camera.main.transform.forward;
            lookDirection = Vector3.ProjectOnPlane(lookDirection, Player.transform.up);

            desiredDirection = Quaternion.FromToRotation(Player.transform.forward, lookDirection) * (Player.transform.right * moveX + Player.transform.forward * moveY);
            desiredDirection = Vector3.ProjectOnPlane(desiredDirection, pushNode.transform.right);
            moveDirection = Vector3.MoveTowards(moveDirection, desiredDirection * movementSpeed, 10 * Time.deltaTime);
        }
    }
    protected override void UpdateAnimator()
    {
        Vector3 speed = rb.velocity;
        speed.y = 0;
        anim.SetFloat("Speed", rb.velocity.magnitude);
        anim.SetFloat("Z", Player.transform.InverseTransformDirection(rb.velocity).z);
    }
    protected override void UpdateIK()
    {
        if (pushNode)
        {
            IK.RightHand.Set(pushNode.rightHand);
            IK.LeftHand.Set(pushNode.leftHand);
        }
    }
    protected override void UpdatePhysics()
    {
        grounded = Physics.CheckCapsule(Player.transform.position, Player.transform.position - Vector3.up * 0.05f, 0.15f, LayerMask.GetMask("Default", "Debris"));
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        rb.AddForce(Player.transform.up * -20f * rb.mass);
        rb.AddForce(Player.transform.InverseTransformVector(pushObject.transform.position - Player.transform.position).x * Player.transform.right * rb.mass * rb.mass);
    }
}