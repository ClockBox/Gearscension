using System;
using System.Collections;
using UnityEngine;

public class AimState : MoveState
{
    private Transform gun;
    private float shootCooldown = 0;

    public AimState(StateManager manager,bool grounded) : base(manager,grounded) { }

    //Transitions
    public override IEnumerator EnterState(PlayerState prevState)
    {
        Debug.Log("AimState : EnterState");

        lookDirection = Camera.main.transform.forward;
        lookDirection = Vector3.ProjectOnPlane(lookDirection, Player.transform.up);
        Player.AimPoint = Player.transform.GetChild(1);

        IK.HeadTrunSpeed = 5;

        gun = Player.weapons[0].transform;

        Player.transform.GetChild(0).position -= Player.transform.right * 0.23f + Player.transform.up * 0.15f;

        yield return ToggleEquip(true);
        yield return base.EnterState(prevState);
    }
    public override IEnumerator ExitState(PlayerState nextState)
    {
        Debug.Log("AimState : ExitState");

        yield return base.ExitState(nextState);

        IK.HeadTrunSpeed = 1;
        
        Player.transform.GetChild(0).position += Player.transform.right * 0.23f + Player.transform.up * 0.15f;
        yield return ToggleEquip(false);
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if ((!Input.GetButton("Aim") && !Player.LeftTrigger.Stay) || Input.GetButtonDown("Roll"))
            stateManager.ChangeState(new UnequipedState(stateManager, grounded));

        else if (shootCooldown <= 0 && (Input.GetButtonUp("Attack") || Player.RightTrigger.Up))
        {
            (Player.weapons[0] as Gun).Shoot();
            shootCooldown = 1;
        }

        else yield return base.HandleInput();

        shootCooldown -= Time.deltaTime;
    }

    //Actions
    IEnumerator ToggleEquip(bool aiming)
    {
        anim.SetBool("aiming", aiming);
        if (aiming)
        {
            yield return ToggleArm(aiming);

            GameManager.Instance.AudioManager.AudioPlayer = Player.SFX;
            GameManager.Instance.AudioManager.playAudio("sfxgundraw");

            yield return ToggleGun(Player.GunHolster, Player.AimPoint);
            gun.parent = Player.AimPoint; 
        }
        else
        {
            yield return ToggleGun(Player.AimPoint, Player.GunHolster);

            GameManager.Instance.AudioManager.AudioPlayer = Player.SFX;
            GameManager.Instance.AudioManager.playAudio("sfxgunputaway");

            yield return ToggleArm(aiming);
            gun.parent = Player.GunHolster;
        }
        gun.localPosition = Vector3.zero;
        gun.localRotation = Quaternion.identity;
    }

    private IEnumerator ToggleArm(bool aiming)
    {
        int start = !aiming ? 1 : 0;
        int end = aiming ? 1 : 0;

        elapsedTime = 0;
        while (elapsedTime <= 1)
        {
            base.UpdateAnimator();
            base.UpdatePhysics();
            UpdateIK();

            IK.LeftHand.weight = Mathf.Lerp(start, end, elapsedTime);
            CameraController.Zoom = Mathf.Lerp(start, end, elapsedTime);

            elapsedTime += Time.deltaTime * 4;
            yield return null;
        }
    }

    private IEnumerator ToggleGun(Transform start, Transform end)
    {
        elapsedTime = 0;
        while (elapsedTime <= 1)
        {
            gun.position = Vector3.Lerp(start.position, end.position, elapsedTime);
            gun.rotation = Quaternion.Lerp(start.rotation, end.rotation, elapsedTime);

            base.UpdateMovement();
            base.UpdateAnimator();
            base.UpdatePhysics();
            UpdateIK();

            elapsedTime += Time.deltaTime * 4;
            yield return null;
        }
        gun.position = end.position;
        gun.rotation = end.rotation;
    }

    //State Updates
    protected override void UpdateMovement()
    {
        base.UpdateMovement();

        Player.transform.LookAt(Player.transform.position + lookDirection);
        gun.position = Player.AimPoint.position + Vector3.up * -Player.AimPoint.rotation.z;
    }
    protected override void UpdateIK()
    {
        base.UpdateIK();

        //Hand transform
        IK.LeftHand.position = Player.weapons[0].Grip(0).position;
        IK.LeftHand.rotation = Player.weapons[0].Grip(0).rotation;
    }

    public override void OnTriggerEnter(Collider other) { }
}
