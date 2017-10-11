using System.Collections;
using UnityEngine;

public class WalkingState : PlayerState
{
    protected Vector3 desiredDirection;
    protected Vector3 lookDirection;
    protected static Vector3 moveDirection;

    protected float movementSpeed = 5;
    protected float moveX = 0;
    protected float moveY = 0;

    float jumpForce = 6;
    float fallTimer = 0;

    public WalkingState(StateManager manager,bool isGrounded) : base(manager)
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
    private void Jump()
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
        
        if (Input.GetKey(KeyCode.LeftShift))
        movementSpeed = 5;
        else movementSpeed = 8;

        lookDirection = Camera.main.transform.forward;
        lookDirection = Vector3.ProjectOnPlane(lookDirection, Player.transform.up);

        desiredDirection = Quaternion.FromToRotation(Player.transform.forward, lookDirection) * (Player.transform.right * moveX + Player.transform.forward * moveY);
        moveDirection = Vector3.MoveTowards(moveDirection, desiredDirection * movementSpeed, 10 * Time.deltaTime);

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
        grounded = Physics.CheckCapsule(Player.transform.position, Player.transform.position - Vector3.up * 0.05f, 0.18f, LayerMask.GetMask("Ground", "Debris"));

        if (grounded)
        {
            rb.AddForce(Player.transform.up * -20f * rb.mass);
            rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        }
        else
        {
            rb.AddForce(Player.transform.up * -9.81f * rb.mass);
            rb.AddForce(moveDirection/2 * rb.mass);
        }
    }

    //TriggerFucntions
    public override void OnTriggerEnter(Collider other)
    {
        if (!InTransition && canClimb)
        {

            if (other.CompareTag("ClimbingNode") || other.CompareTag("HookNode"))
            {
                if (Vector3.Dot(other.transform.forward, Player.transform.forward) > 0)
                    stateManager.ChangeState(new ClimbState(stateManager, other.GetComponent<ClimbingNode>()));
            }
            else if (grounded && other.CompareTag("ClimbingEdge") && moveDirection.magnitude < 5.5f)
            {
                if (Vector3.Dot(other.transform.forward, Player.transform.forward) < 0)
                    stateManager.ChangeState(new ClimbState(stateManager, other.GetComponent<ClimbingEdge>()));
            }
        }
    }
}
