using System.Collections;
using UnityEngine;

public class UnequipedState : WalkingState
{
    public UnequipedState(StateManager manager, bool grounded) : base(manager, grounded) { }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (Input.GetButtonDown("Attack") || Input.GetButtonDown("Equip"))
            stateManager.ChangeState(new CombatState(stateManager,grounded));

        else if (Input.GetButton("Aim"))
            stateManager.ChangeState(new AimState(stateManager,grounded));

        else
            yield return base.HandleInput();
    }

    public override void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Action") && !InTransition)
        {
            if (other.CompareTag("CarryNode"))
                stateManager.ChangeState(new CarryState(stateManager, other.GetComponent<CarryNode>(), grounded));
            else if (other.CompareTag("Pushable"))
                stateManager.ChangeState(new PushState(stateManager, other.gameObject));
        }
    }
}
