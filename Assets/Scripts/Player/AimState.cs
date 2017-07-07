using System;
using System.Collections;
using UnityEngine;

public class AimState : WalkingState
{
    float equipTime = 1.233f;

    public AimState(StateManager manager,bool grounded) : base(manager,grounded) { }

    //Transitions
    public override IEnumerator EnterState()
    {
        yield return base.EnterState();
        lookDirection = Camera.main.transform.forward;
        lookDirection = Vector3.ProjectOnPlane(lookDirection, Player.transform.up);

        CameraController.Zoomed = true;
        Player.transform.GetChild(0).position -= Player.transform.right * 0.2f + Player.transform.up * 0.2f;

        yield return Player.StartCoroutine(ToggleGun());
    }
    public override IEnumerator ExitState()
    {
        yield return base.ExitState();
        CameraController.Zoomed = false;
        Player.transform.GetChild(0).position += Player.transform.right * 0.2f + Player.transform.up * 0.2f;

        yield return stateManager.StartCoroutine(ToggleGun());
    }

    //Actions
    IEnumerator ToggleGun()
    {
        anim.SetBool("aiming", !anim.GetBool("aiming"));

        short start = Convert.ToInt16(!anim.GetBool("aiming"));
        short end = Convert.ToInt16(anim.GetBool("aiming"));

        Player.StartCoroutine(Player.ToggleWeapon(0, start, 1));

        //Should be replaced by animation
        elapsedTime = 0;
        while (elapsedTime <= equipTime)
        {
            IK.LeftHand.position = Player.transform.GetChild(0).position - Player.transform.up * 0.2f + Camera.main.transform.forward * 0.5f;
            IK.LeftHand.rotation = Camera.main.transform.rotation * Quaternion.LookRotation(Vector3.forward, -Vector3.right);
            IK.LeftHand.weight = Mathf.Lerp(start, end, elapsedTime);

            base.UpdateMovement();
            base.UpdateAnimator();
            base.UpdatePhysics();
            base.UpdateIK();

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (!Input.GetButton("Aim"))
            stateManager.ChangeState(new UnequipedState(stateManager, grounded));

        else
            yield return base.HandleInput();
    }

    //State Updates
    protected override void UpdateIK()
    {
        IK.LeftHand.position = Player.transform.GetChild(0).position - Player.transform.up * 0.2f + Camera.main.transform.forward * 0.5f;
        IK.LeftHand.rotation = Camera.main.transform.rotation * Quaternion.LookRotation(Vector3.forward, -Vector3.right);
    }
}
