using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnequipedState : MoveState
{
    public UnequipedState(StateManager manager, bool grounded) : base(manager, grounded) { }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (Input.GetButtonDown("Attack") || Input.GetButtonDown("Equip") || Player.RightTrigger.Down)
            stateManager.ChangeState(new CombatState(stateManager, grounded));

        else if (Player.GunUpgrades >= 0 && (Input.GetButton("Aim") || Player.LeftTrigger.Stay))
            stateManager.ChangeState(new AimState(stateManager, grounded));

        else yield return base.HandleInput();
    }

    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        if (Input.GetButtonDown("Action") && !InTransition)
        {
            if (other.CompareTag("Pushable"))
                stateManager.ChangeState(new PushState(stateManager, other.GetComponent<HandNode>()));
            else if (other.CompareTag("CarryNode"))
                stateManager.ChangeState(new CarryState(stateManager, other.GetComponent<HandNode>(), grounded));
        }
    }
}
