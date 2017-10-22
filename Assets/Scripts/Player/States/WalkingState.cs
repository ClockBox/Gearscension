﻿using System.Collections;
using UnityEngine;

public class MoveState : PlayerState
{
    protected Vector3 desiredDirection;
    protected Vector3 lookDirection;
    protected static Vector3 moveDirection;

    protected float movementSpeed = 5;
    protected float moveX = 0;
    protected float moveY = 0;
    
    protected ClimbingNode hookNode;
    protected bool hooked = false;

    private float jumpForce = 7;
    private float fallTimer = 0;

    public MoveState(StateManager manager,bool isGrounded) : base(manager)
    {
        grounded = isGrounded;
    }

    //Transitions
    public override IEnumerator EnterState()
    {
        moveDirection = moveDirection.magnitude * Player.transform.forward;
        if (canClimb == false)
            moveDirection = Vector3.zero;
        anim.SetBool("isGrounded", grounded);
        yield return base.EnterState();
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (!grounded && fallTimer > 3.0f)
            stateManager.ChangeState(new FallState(stateManager));

        else if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
                Jump();
            else
                canClimb = true;
        }

        if (Input.GetButtonDown("Hook"))
            Player.StartCoroutine(ThrowHook(Player.FindHookTarget("HookNode")));

        else if (grounded && Input.GetButtonDown("Roll"))
            yield return Dodge();
    }

    //State Actions
    private IEnumerator Dodge()
    {
        anim.SetTrigger("roll");
        elapsedTime = 0;
        while (elapsedTime <= 1.3f)
        {
            moveDirection = moveDirection.normalized * anim.velocity.magnitude * 3f;
            UpdateIK();
            movementSpeed = 10f;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    protected void Jump()
    {
        anim.SetBool("isGrounded", false);
        rb.velocity = -new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Player.transform.up * jumpForce * rb.mass, ForceMode.Impulse);
        grounded = false;
    }

    //State Updates
    protected override void UpdateMovement()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        
        if (Input.GetButton("Sprint"))
            movementSpeed = 5;
        else movementSpeed = 8;

        lookDirection = Camera.main.transform.forward;
        lookDirection = Vector3.ProjectOnPlane(lookDirection, Player.transform.up);

        desiredDirection = Quaternion.FromToRotation(Player.transform.forward, lookDirection) * (Player.transform.right * moveX + Player.transform.forward * moveY);
        moveDirection = Vector3.MoveTowards(moveDirection, desiredDirection * movementSpeed, 50 * Time.deltaTime);

        if (desiredDirection.magnitude > 0)
            moveDirection = Vector3.RotateTowards(moveDirection, desiredDirection + lookDirection * 0.01f, 20 * Time.deltaTime, 0);

        if (moveDirection.magnitude > movementSpeed)
            moveDirection = moveDirection.normalized * movementSpeed;

        if (grounded)
        {
            Player.transform.LookAt(Player.transform.position + moveDirection, Player.transform.up);
            canClimb = true;
            fallTimer = 0;
        }
        else fallTimer += Time.deltaTime;
    }
    protected override void UpdateAnimator()
    {
        Vector3 speed = rb.velocity;
        speed.y = 0;
        anim.SetFloat("Speed", speed.magnitude);
        anim.SetBool("isGrounded", grounded);
    }
    protected override void UpdatePhysics()
    {
        grounded = Physics.CheckCapsule(Player.transform.position, Player.transform.position - Vector3.up * 0.05f, 0.15f, LayerMask.GetMask("Default", "Debris"));

        if (grounded)
        {
            rb.AddForce(Player.transform.up * -20f * rb.mass);
            rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        }
        else
        {
            rb.AddForce(Player.transform.up * -9.81f * rb.mass);
            rb.AddForce(-moveDirection / 4 * rb.mass);
        }
    }
    
    #region Hook Functions
    protected IEnumerator ThrowHook(GameObject node)
    {
        if (!node)
            yield break;

        InTransition = true;
        anim.SetBool("hook", true);

        Transform sword = Player.weapons[1].transform;
        sword.parent = null;

        desiredDirection = node.transform.position - Player.transform.position;

        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            sword.position = Vector3.Lerp(sword.position, node.transform.position - node.transform.forward * 0.3f + node.transform.right * 0.1f, elapsedTime);
            sword.rotation = Quaternion.Lerp(sword.rotation, node.transform.rotation * new Quaternion(0, -1, 0, 1), elapsedTime);
            elapsedTime += Time.deltaTime;

            base.UpdateMovement();
            base.UpdateAnimator();
            base.UpdateIK();
            base.UpdatePhysics();

            yield return null;
        }
        sword.parent = node.transform;

       CarryNode pullObject;
         if (pullObject = node.GetComponentInParent<CarryNode>())
            yield return HookPull(pullObject);
        else
            yield return HookTravel(node.GetComponent<ClimbingNode>());
    }
    protected IEnumerator HookTravel(ClimbingNode hook)
    {
        hookNode = hook;
        if (hookNode)
        {
            IK.RightHand.position = hook.rightHand.position;
            IK.RightHand.rotation = hook.rightHand.rotation;
        }

        anim.SetBool("climbing", true);

        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            Player.transform.position = Vector3.Lerp(Player.transform.position, hook.PlayerPosition, elapsedTime);
            Player.transform.rotation = Quaternion.Lerp(Player.transform.rotation, hook.transform.rotation, elapsedTime);
            
            IK.GlobalWeight = elapsedTime;
            IK.SetIKPositions(Player.weapons[1].transform, hook.leftHand, hook.rightFoot, hook.leftFoot);
            IK.RightFoot.weight = 0;
            IK.LeftFoot.weight = 0;
            IK.HeadWeight = 0;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hooked = true;
        if (hook.FreeHang)
            Player.transform.localEulerAngles = new Vector3(0, Player.transform.localEulerAngles.y, Player.transform.localEulerAngles.z);

        rb.velocity = Vector3.zero;
        InTransition = false;
    }

    protected IEnumerator HookPull(CarryNode pulledObject)
    {
        Vector3 startPos = pulledObject.transform.position;
        Vector3 offsetPos = Player.transform.position + (Player.transform.up * 1.1f) + (Player.transform.forward * 0.3f);

        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            pulledObject.transform.position = Vector3.Lerp(startPos, offsetPos, elapsedTime);
            pulledObject.transform.rotation = Quaternion.Lerp(pulledObject.transform.rotation, Player.transform.rotation, elapsedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        InTransition = false;
    }
    #endregion

    //TriggerFucntions
    public override void OnTriggerEnter(Collider other)
    {
        if (!InTransition && canClimb)
        {

            if (other.CompareTag("ClimbingNode") || other.CompareTag("HookNode"))
            {
                if (Vector3.Dot(other.transform.forward, Player.transform.forward) > 0)
                {
                    moveDirection = Vector3.zero;
                    stateManager.ChangeState(new ClimbState(stateManager, other.GetComponent<ClimbingNode>()));
                }
            }
            else if (grounded && other.CompareTag("ClimbingEdge") && moveDirection.magnitude < 5.5f)
            {
                if (Vector3.Dot(other.transform.forward, Player.transform.forward) < 0)
                {
                    moveDirection = Vector3.zero;
                    stateManager.ChangeState(new ClimbState(stateManager, other.GetComponent<ClimbingEdge>()));
                }
            }
        }
    }
}
