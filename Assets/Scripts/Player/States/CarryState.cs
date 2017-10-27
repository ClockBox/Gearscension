using System.Collections;
using UnityEngine;

public class CarryState : MoveState
{
    Transform carryObject;
    CarryNode carryNode;

    Vector3 OffsetPosition;

    public CarryState(StateManager manager, CarryNode carryObject, bool grounded) : base(manager, grounded)
    {
        this.carryNode = carryObject;
    }

    //Transitions
    public override IEnumerator EnterState(PlayerState prevState)
    {
        IK.HeadTrunSpeed = 5;
        IK.RightHand.weight = 0.8f;
        IK.LeftHand.weight = 0.8f;
        
        carryNode.rigidBody.isKinematic = true;
        carryNode.Collider.enabled = false;

        carryObject = carryNode.gameObject.transform.parent;
        carryObject.parent = Player.transform;

        carryObject.position = Player.transform.position + (Player.transform.up * 1.1f) + (Player.transform.forward * 0.3f);
        carryObject.rotation = Player.transform.rotation;

        yield return base.EnterState(prevState);
    }
    public override IEnumerator ExitState(PlayerState nextState)
    {
        yield return base.ExitState(nextState);
        IK.HeadTrunSpeed = 1;
        IK.RightHand.weight = 0;
        IK.LeftHand.weight = 0;
        carryObject.parent = null;
        carryNode.rigidBody.isKinematic = false;
        carryNode.Collider.enabled = true;
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (Input.GetButtonDown("Equip"))
            stateManager.ChangeState(new CombatState(stateManager, grounded));

        else if (Input.GetButtonDown("Action"))
            stateManager.ChangeState(new UnequipedState(stateManager, grounded));

        else if (Input.GetButton("Aim"))
            stateManager.ChangeState(new AimState(stateManager, grounded));

        else
            yield return base.HandleInput();
    }

    //State Updates
    protected override void UpdateIK()
    {
        base.UpdateIK();

        IK.RightHand.weight = 0.8f;
        IK.RightHand.position = carryNode.rightHand.position;
        IK.RightHand.rotation = carryNode.rightHand.rotation;

        IK.LeftHand.weight = 0.8f;
        IK.LeftHand.position = carryNode.leftHand.position;
        IK.LeftHand.rotation = carryNode.leftHand.rotation;
    }

    //Trigger Functions
    public override void OnTriggerEnter(Collider other) { }
}
