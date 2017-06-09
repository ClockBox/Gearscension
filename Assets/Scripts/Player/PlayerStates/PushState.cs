using UnityEngine;
using System.Collections;

public class PushState : PlayerState
{
    Rigidbody PushObject;

    Vector3 rightHandOffset = Vector3.zero;
    Vector3 leftHandOffset = Vector3.zero;

    public PushState(GameObject pushObject) : base()
    {
        IK.RightHand.weight = 0.9f;
        IK.LeftHand.weight = 0.9f;

        PushObject = pushObject.GetComponent<Rigidbody>();
        PushObject.constraints = RigidbodyConstraints.FreezeRotation;
        PushObject.transform.parent = Player.transform;

        anim.SetBool("hasSword", false);
        anim.SetBool("aiming", false);
        anim.SetBool("pushing", true);

        FindHandOffset();
    }
	
    public override CharacterState UpdateState()
    {
        HandleInput();
        return HandleStateChange();
    }

    protected override CharacterState HandleStateChange()
    {
        if (Input.GetButtonDown("Action"))
            return new GroundedState();

        return null;
    }

    protected override void HandleInput()
    {
        base.HandleInput();

        MovementSpeed = rb.mass / PushObject.mass;

        moveDirection = Quaternion.FromToRotation(Player.transform.forward, lookDirection) * (Player.transform.right * X + Player.transform.forward * Z) * MovementSpeed;
        if (moveDirection.magnitude > MovementSpeed)
            moveDirection = moveDirection.normalized * MovementSpeed;

        anim.SetFloat("Speed", rb.velocity.magnitude);
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        rb.AddForce(Player.transform.up * -20f * rb.mass);

        PushObject.velocity = rb.velocity;
    }

    public override void UpdateIK()
    {
        RaycastHit RightHit;
        Transform RightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot);
        if (Physics.Raycast(RightFoot.position + Player.transform.up * 0.1f, -Player.transform.up, out RightHit, 0.55f))
        {
            IK.RightFoot.position = RightHit.point + Player.transform.up * 0.12f;
            IK.RightFoot.rotation = Quaternion.FromToRotation(Player.transform.up, RightHit.normal) * Player.transform.rotation;
        }
        else IK.RightFoot.weight = 0;

        RaycastHit LeftHit;
        Transform LeftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        if (Physics.Raycast(LeftFoot.position + Player.transform.up * 0.1f, -Player.transform.up, out LeftHit, 0.55f))
        {
            IK.LeftFoot.rotation = Quaternion.FromToRotation(Player.transform.up, LeftHit.normal) * Player.transform.rotation;
        }
        else IK.LeftFoot.weight = 0;

        IK.RightHand.position = PushObject.transform.position + rightHandOffset;
        IK.LeftHand.position = PushObject.transform.position + leftHandOffset;
    }

    private void FindHandOffset()
    {
        if (PushObject)
        {
            //RayCast- Right Hand
            Vector3 rightRayStart = Player.transform.up * 1.4f + Player.transform.right * 0.5f;
            rightRayStart.y = Mathf.Clamp(rightRayStart.y, 0.5f, (PushObject.transform.position.y-Player.transform.position.y) + PushObject.transform.localScale.y/2);
            Vector3 rightRayDirection = Player.transform.forward - Player.transform.right * 0.5f;
            RaycastHit rightHit;

            if (Physics.Raycast(Player.transform.position + rightRayStart, rightRayDirection, out rightHit, 1f))
            {
                IK.RightHand.position = rightHit.point - Player.transform.up * 0.1f - Player.transform.forward * 0.1f;
                IK.RightHand.rotation = Quaternion.FromToRotation(Player.transform.up, rightHit.normal) * Player.transform.rotation;
                IK.RightHand.weight = 1f;
            }

            rightHandOffset = IK.RightHand.position - PushObject.transform.position;

            //RayCast- left Hand
            Vector3 leftRayStart = Player.transform.up * 1.4f - Player.transform.right * 0.5f;
            leftRayStart.y = Mathf.Clamp(leftRayStart.y, 1, (PushObject.transform.position.y - Player.transform.position.y) + PushObject.transform.localScale.y / 2);
            Vector3 leftRayDirection = Player.transform.forward + Player.transform.right * 0.5f;
            RaycastHit leftHit;

            if (Physics.Raycast(Player.transform.position + leftRayStart, leftRayDirection, out leftHit, 1f))
            {
                IK.LeftHand.position = leftHit.point - Player.transform.up * 0.1f - Player.transform.forward * 0.1f;
                IK.LeftHand.rotation = Quaternion.FromToRotation(Player.transform.up, leftHit.normal) * Player.transform.rotation;
                IK.LeftHand.weight = 1f;
            }

            leftHandOffset = IK.LeftHand.position - PushObject.transform.position;
        }
    }

    public override IEnumerator ExitState()
    {
        PushObject.transform.parent = null;
        anim.SetBool("pushing", false);
        PushObject.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        PushObject.velocity = Vector3.zero;
        IK.RightHand.weight = 0;
        IK.LeftHand.weight = 0;
        yield return null;
    }

    public override CharacterState OnTriggerEnter(Collider other) { return null; }
    public override CharacterState OnTriggerStay(Collider other) { return null; }
    public override CharacterState OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pushable"))
            return new EquipmentState();
        return null;
    }
}