using System.Collections;
using UnityEngine;

public class FallState : PlayerState
{
    public FallState(StateManager manager) : base(manager) { }

    //Transitions
    public override IEnumerator EnterState(PlayerState prevState)
    {
        grounded = false;
        anim.SetTrigger("falling");
        yield return base.EnterState(prevState);
    }
    public override IEnumerator ExitState(PlayerState nextState)
    {
        yield return base.ExitState(nextState);
        rb.velocity = Vector3.zero;
        yield return null;
        GameManager.Instance.AudioManager.AudioPlayer = Player.SFX;
        GameManager.Instance.AudioManager.playAudio("sfxbodyfallconcrete2");
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (grounded)
        {
            anim.SetBool("isGrounded", true);
            yield return new WaitForSeconds(3.5f);
            stateManager.ChangeState(new UnequipedState(stateManager, true));
        }

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
