using UnityEngine;
using System.Collections;

public class GroundedState : PlayerState
{
    Vector3 desiredDirection;

    public GroundedState() : base()
    {
        MovementSpeed = 5f;
        canSprint = true;
        canClimb = true;

        IK.headWeight = 1;
        IK.RightFoot.weight = 0;
        IK.LeftFoot.weight = 0;

        anim.SetBool("isGrounded", true);
    }

    public override CharacterState UpdateState()
    {
        HandleInput();
        return HandleStateChange();
    }

    protected override CharacterState HandleStateChange()
    {
        if (!Grounded)
            return new FallState();

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
            return new FallState();
        }

        if (Input.GetButtonDown("Roll") && moveDirection.magnitude > Mathf.Epsilon)
            return new DodgeAction(desiredDirection);
        return null;
    }

    protected override void HandleInput()
    {
        base.HandleInput();

        if (Input.GetButton("Sprint") && canSprint)
            MovementSpeed = 8;
        else MovementSpeed = 5;

        desiredDirection = Quaternion.FromToRotation(Player.transform.forward, lookDirection) * (Player.transform.right * X + Player.transform.forward * Z);
        moveDirection = Vector3.MoveTowards(moveDirection, desiredDirection * MovementSpeed, 10 * Time.deltaTime);

        if (desiredDirection.magnitude > 0)
            moveDirection = Vector3.RotateTowards(moveDirection, desiredDirection + lookDirection * 0.01f, 20 * Time.deltaTime, 0);

        Debug.DrawLine(Player.transform.position, moveDirection + Player.transform.position, Color.red, 0.05f);
        Player.transform.LookAt(Player.transform.position + moveDirection, Player.transform.up);

        if (desiredDirection.magnitude == 0)
            anim.SetFloat("turnAngle", SignedAngle(Player.transform.forward, lookDirection,Player.transform.up));
        
        if (moveDirection.magnitude > MovementSpeed)
            moveDirection = moveDirection.normalized * MovementSpeed;

        anim.SetFloat("Speed", rb.velocity.magnitude);
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        rb.AddForce(Player.transform.up * -20f * rb.mass);
    }

    public override void UpdateIK()
    {
        base.UpdateIK();

        RaycastHit RightHit;
        Transform RightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot);
        if (Physics.Raycast(RightFoot.position + Player.transform.up * 0.1f, -Player.transform.up, out RightHit, 0.5f))
        {
            IK.RightFoot.position = RightHit.point + Player.transform.up * 0.12f;
            IK.RightFoot.rotation = Quaternion.FromToRotation(Player.transform.up, RightHit.normal) * Player.transform.rotation;
        }
        else IK.RightFoot.weight = 0;

        RaycastHit LeftHit;
        Transform LeftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        if (Physics.Raycast(LeftFoot.position + Player.transform.up * 0.1f, -Player.transform.up, out LeftHit, 0.5f))
        {
            IK.LeftFoot.position = LeftHit.point + Player.transform.up * 0.12f;
            IK.LeftFoot.rotation = Quaternion.FromToRotation(Player.transform.up, LeftHit.normal) * Player.transform.rotation;
        }
        else IK.LeftFoot.weight = 0;

        if (RightHit.collider || LeftHit.collider)
            Grounded = true;
        else Grounded = false;
    }

    public void Jump()
    {
        anim.SetTrigger("jump");
        rb.AddForce(Player.transform.up * jumpForce * rb.mass, ForceMode.Impulse);
    }

    public override CharacterState OnTriggerEnter(Collider other)
    {
        if (canClimb)
        {
            if (other.gameObject.CompareTag("ClimbingNode"))
                return new ClimbState(other.gameObject.GetComponent<ClimbingNode>());
            if (other.gameObject.CompareTag("ClimbingEdge"))
                return new ClimbDownAction(other.gameObject.GetComponent<ClimbingEdge>());
        }
        return null;
    }
}