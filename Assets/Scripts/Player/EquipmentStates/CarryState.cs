using UnityEngine;
using System.Collections;

public class CarryState : EquipmentState
{
    Transform carryObject;
    CarryNode carryNode;

    Vector3 OffsetPosition;

    public CarryState(CarryNode node) : base()
    {
        IK.RightHand.weight = 0.8f;
        IK.LeftHand.weight = 0.8f;

        carryNode = node;
        carryNode.rigidBody.isKinematic = true;
        carryNode.Collider.enabled = false;
        carryNode.delayPickup(0.1f);

        carryObject = node.gameObject.transform.parent;
        carryObject.parent = Player.transform;
    }

    public override CharacterState UpdateState()
    {
        
        return HandleStateChange();
    }

    protected override CharacterState HandleStateChange()
    {
        if (anim.GetBool("climbing"))
            return new EquipmentState();

        if (carryNode.Active)
        {
            if (Input.GetButtonDown("Action") || Input.GetButtonDown("Equip") || Input.GetButton("Attack") || rightTriggerState == DOWN)
            {
                ExitState();
                return new EquipmentState();
            }
        }
        return null;
    }

    public override void UpdateIK()
    {
        OffsetPosition = Player.transform.position + (Player.transform.up * 1.1f) + (Player.transform.forward * 0.3f);

        IK.RightHand.weight = 0.8f;
        IK.RightHand.position = carryNode.rightHand.position;
        IK.RightHand.rotation = carryNode.rightHand.rotation;

        IK.LeftHand.weight = 0.8f;
        IK.LeftHand.position = carryNode.leftHand.position;
        IK.LeftHand.rotation = carryNode.leftHand.rotation;

        carryObject.rotation = Player.transform.rotation;
        carryObject.position =  OffsetPosition;
    }

    public override void ExitState()
    {
        IK.RightHand.weight = 0;
        IK.LeftHand.weight = 0;
        carryObject.parent = null;
        carryNode.rigidBody.isKinematic = false;
        carryNode.Collider.enabled = true;
        carryNode.delayPickup(0.5f);
    }

    public override CharacterState OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ClimbingNode"))
            return new EquipmentState();
        return null;
    }
    public override CharacterState OnTriggerStay(Collider other) { return null; }
}