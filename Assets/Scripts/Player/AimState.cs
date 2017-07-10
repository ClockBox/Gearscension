using System;
using System.Collections;
using UnityEngine;

public class AimState : WalkingState
{
    float equipTime = 1.233f;
    float bulletScale = 1;
    float handToGunWeight;

    public AimState(StateManager manager,bool grounded) : base(manager,grounded) { }

    //Transitions
    public override IEnumerator EnterState()
    {
        yield return base.EnterState();
        lookDirection = Camera.main.transform.forward;
        lookDirection = Vector3.ProjectOnPlane(lookDirection, Player.transform.up);

        CameraController.Zoomed = true;
        Player.transform.GetChild(0).position -= Player.transform.right * 0.15f + Player.transform.up * 0.15f;

        yield return Player.StartCoroutine(ToggleGun());
    }
    public override IEnumerator ExitState()
    {
        yield return base.ExitState();

        CameraController.Zoomed = false;
        Player.transform.GetChild(0).position += Player.transform.right * 0.15f + Player.transform.up * 0.15f;

        yield return stateManager.StartCoroutine(ToggleGun());
        IK.LeftHand.weight = 0;
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (!Input.GetButton("Aim"))
            stateManager.ChangeState(new UnequipedState(stateManager, grounded));

        if (Input.GetButton("Attack"))
            bulletScale += Time.deltaTime;

        else if (Input.GetButtonUp("Attack"))
        {
            (Player.weapons[0] as Gun).Shoot(bulletScale);
            bulletScale = 1;
        }

        else
            yield return base.HandleInput();
    }

    //Actions
    IEnumerator ToggleGun()
    {
        anim.SetBool("aiming", !anim.GetBool("aiming"));

        int start = !anim.GetBool("aiming") ? 1 : 0;
        int end = anim.GetBool("aiming") ? 1 : 0;

        Player.StartCoroutine(Player.ToggleWeapon(0, start, 1));

        //Should be replaced by animation
        elapsedTime = 0;
        while (elapsedTime <= equipTime)
        {
            IK.LeftHand.weight = Mathf.Lerp(start, end, elapsedTime);
            handToGunWeight = Mathf.Lerp(start, end, elapsedTime);

            base.UpdateMovement();
            base.UpdateAnimator();
            base.UpdatePhysics();
            base.UpdateIK();

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        IK.LeftHand.weight = 1;
    }

    //State Updates
    protected override void UpdateMovement()
    {
        base.UpdateMovement();

        if (IK.LeftHand.weight == 1)
        {
            Vector3 desiredPoint = Player.transform.GetChild(0).position - Player.transform.up * 0.15f + Camera.main.transform.forward * 0.5f;

            //Gun transform
            Player.weapons[0].transform.position = Vector3.Lerp(Player.weapons[0].transform.position, desiredPoint, 0.8f);
            Player.weapons[0].transform.rotation = Quaternion.Lerp(Player.weapons[0].transform.rotation, Camera.main.transform.rotation, 0.8f);
        }
    }

    protected override void UpdateIK()
    {
        //Hand transform
        Vector3 tempPos = Player.transform.GetChild(0).position - Player.transform.up * 0.2f + Camera.main.transform.forward * 0.5f;
        Quaternion tempRot = Camera.main.transform.rotation * Quaternion.LookRotation(Vector3.forward, -Vector3.right);

        IK.LeftHand.position = Vector3.Lerp(tempPos,Player.weapons[0].Grip(0).position,handToGunWeight);
        IK.LeftHand.rotation =  Quaternion.Lerp(tempRot, Player.weapons[0].Grip(0).rotation, handToGunWeight);
    }
}
