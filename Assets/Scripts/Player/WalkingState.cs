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

    protected static bool grounded = true;

    float jumpForce = 6;

    public WalkingState(StateManager manager,bool isGrounded) : base(manager)
    {
        grounded = isGrounded;
    }

    //Transitions
    public override IEnumerator EnterState()
    {
        moveDirection = moveDirection.magnitude * Player.transform.forward;
        anim.SetBool("isGrounded", grounded);
        yield return base.EnterState();
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (!grounded && elapsedTime > 3.0f)
            stateManager.ChangeState(new FallState(stateManager));

        else if (grounded && Input.GetButtonDown("Jump"))
            Jump();

        else if (Input.GetButtonDown("Roll"))
            yield return Dodge();
    }

    //State Actions
    private IEnumerator Dodge()
    {
        anim.SetTrigger("roll");
        elapsedTime = 0;
        while (elapsedTime <= 1.3f)
        {
            moveDirection = moveDirection.normalized * anim.velocity.magnitude * 2f;
            UpdateIK();
             movementSpeed = 10f;
            elapsedTime += Time.deltaTime;
             yield return null;
        }
    }
    private void Jump()
    {
        anim.SetBool("isGrounded", false);
        rb.AddForce(Player.transform.up * jumpForce * rb.mass, ForceMode.Impulse);
        grounded = false;
    }

    //State Updates
    protected override void UpdateMovement()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        if (grounded)
        {
            elapsedTime = 0;

            if (Input.GetKey(KeyCode.LeftShift))
                movementSpeed = 8;
            else movementSpeed = 5;

            lookDirection = Camera.main.transform.forward;
            lookDirection = Vector3.ProjectOnPlane(lookDirection, Player.transform.up);

            desiredDirection = Quaternion.FromToRotation(Player.transform.forward, lookDirection) * (Player.transform.right * moveX + Player.transform.forward * moveY);
            moveDirection = Vector3.MoveTowards(moveDirection, desiredDirection * movementSpeed, 10 * Time.deltaTime);

            if (desiredDirection.magnitude > 0)
                moveDirection = Vector3.RotateTowards(moveDirection, desiredDirection + lookDirection * 0.01f, 20 * Time.deltaTime, 0);

            Player.transform.LookAt(Player.transform.position + moveDirection, Player.transform.up);

            if (moveDirection.magnitude > movementSpeed)
                moveDirection = moveDirection.normalized * movementSpeed;
        }
    }
    protected override void UpdateAnimator()
    {
        Vector3 speed = rb.velocity;
        speed.y = 0;
        anim.SetFloat("Speed", speed.magnitude);
        anim.SetBool("isGrounded", grounded);
    }
    protected override void UpdateIK()
    {
        if (grounded)
        {
            IK.RightFoot.weight = anim.GetFloat("RightFootWeight");
            IK.LeftFoot.weight = anim.GetFloat("LeftFootWeight");
        }
        else
        {
            IK.LeftFoot.weight = 0;
            IK.RightFoot.weight = 0;
        }
    }
    protected override void UpdatePhysics()
    {
        if (grounded)
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

            if (!RightHit.collider && !RightHit.collider)
                grounded = false;
        }
        else
        {
            rb.AddForce(Player.transform.up * -9.81f * rb.mass);

            if (Physics.Raycast(Player.transform.position + (Player.transform.up * 0.5f) - (Player.transform.forward * 0.3f) - (Player.transform.right * 0.3f), -Player.transform.up, 0.55f) ||
                Physics.Raycast(Player.transform.position + (Player.transform.up * 0.5f) + (Player.transform.forward * 0.3f) + (Player.transform.right * 0.3f), -Player.transform.up, 0.55f))
                grounded = true;
        }
    }

    //TriggerFucntions
    public override void OnTriggerEnter(Collider other)
    {
        if (!inTransition)
        {
            if (other.CompareTag("ClimbingNode"))
                stateManager.ChangeState(new ClimbState(stateManager, other.GetComponent<ClimbingNode>()));
            else if (grounded && other.CompareTag("ClimbingEdge"))
                stateManager.ChangeState(new ClimbState(stateManager, other.GetComponent<ClimbingEdge>()));
        }
    }
}
