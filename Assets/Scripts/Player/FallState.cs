using System.Collections;
using UnityEngine;

public class FallState : PlayerState
{
    bool grounded = false;

    public FallState(StateManager manager) : base(manager) { }

    //Transitions
    public override IEnumerator EnterState()
    {
        anim.SetTrigger("falling");
        yield return base.EnterState();
    }
    public override IEnumerator ExitState()
    {
        yield return base.ExitState();
        rb.velocity = Vector3.zero;
        yield return null;
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (grounded)
            stateManager.ChangeState(new UnequipedState(stateManager, true));

        yield return null;
    }

    //State Updates
    protected override void UpdateIK()
    {
        IK.RightFoot.weight = 0;
        IK.LeftFoot.weight = 0;
    }
    protected override void UpdatePhysics()
    {
        rb.AddForce(Player.transform.up * -9.81f * rb.mass);

        if (Physics.Raycast(Player.transform.position + (Player.transform.up * 0.5f) - (Player.transform.forward * 0.3f) - (Player.transform.right * 0.3f), -Player.transform.up, 0.6f) ||
            Physics.Raycast(Player.transform.position + (Player.transform.up * 0.5f) + (Player.transform.forward * 0.3f) + (Player.transform.right * 0.3f), -Player.transform.up, 0.6f))
            grounded = true;
    }
}
