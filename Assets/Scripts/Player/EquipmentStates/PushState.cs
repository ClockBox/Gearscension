using UnityEngine;
using System.Collections;

public class PushState : EquipmentState
{
    Rigidbody PushObject;

    public PushState(GameObject pushObject) : base()
    {
        IK.RightHand.weight = 0.9f;
        IK.LeftHand.weight = 0.9f;

        PushObject = pushObject.GetComponent<Rigidbody>();
        PushObject.constraints = RigidbodyConstraints.FreezeRotation;

        anim.SetBool("hasSword", false);
        anim.SetBool("aiming", false);
        anim.SetBool("pushing", true);
    }
	
    public override CharacterState UpdateState()
    {
        return HandleStateChange();
    }

    protected override CharacterState HandleStateChange()
    {
        if (anim.GetBool("climbing"))
            return new EquipmentState();

        if (Input.GetAxis("Vertical") <= Mathf.Epsilon || Input.GetAxis("Horizontal") != 0)
            return new EquipmentState();

        if (IK.RightHand.weight == 0 && IK.LeftHand.weight == 0)
            return new EquipmentState();

        return null;
    }

    public override void UpdateIK()
    {
        if (PushObject)
        {
            PushObject.AddForce(moveDirection * rb.mass / PushObject.mass * 2);

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
            else IK.RightHand.weight = 0f;

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
            else IK.LeftHand.weight = 0f;
        }
    }

    public override IEnumerator ExitState()
    {
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