using System;
using System.Collections;
using UnityEngine;

public class AimState : WalkingState
{
    float bulletScale = 1;
    float handToGunWeight;

    private Transform desiredPoint;
    private Transform sword;
    private Transform gun;

    public AimState(StateManager manager,bool grounded) : base(manager,grounded) { }

    //Transitions
    public override IEnumerator EnterState()
    {
        lookDirection = Camera.main.transform.forward;
        lookDirection = Vector3.ProjectOnPlane(lookDirection, Player.transform.up);
        desiredPoint = Player.transform.GetChild(1);

        IK.HeadTrunSpeed = 5;

        gun = Player.weapons[0].transform;
        sword = Player.weapons[1].transform;

        Player.transform.GetChild(0).position -= Player.transform.right * 0.23f + Player.transform.up * 0.15f;

        yield return ToggleEquip(true);
        yield return base.EnterState();
    }
    public override IEnumerator ExitState()
    {
        yield return base.ExitState();

        IK.HeadTrunSpeed = 1;
        
        Player.transform.GetChild(0).position += Player.transform.right * 0.23f + Player.transform.up * 0.15f;
        yield return ToggleEquip(false);
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.X))
            Player.StartCoroutine(ThrowHook(Player.FindHookTarget("CarryNode")));

        if ((!Input.GetButton("Aim") && !Player.LeftTrigger.Stay) || Input.GetButtonDown("Roll"))
            stateManager.ChangeState(new UnequipedState(stateManager, grounded));

        if (Input.GetButton("Attack"))
            bulletScale += Time.deltaTime;

        else if (Input.GetButtonUp("Attack") || Player.RightTrigger.Up)
        {
            (Player.weapons[0] as Gun).Shoot(bulletScale);
            bulletScale = 1;
        }
        else
            yield return base.HandleInput();
    }

    //Actions
    IEnumerator ToggleEquip(bool aiming)
    {
        anim.SetBool("aiming", aiming);
        if (aiming)
        {
            yield return ToggleArm(aiming);
            yield return ToggleGun(aiming);
            gun.parent = desiredPoint;
        }
        else
        {
            yield return ToggleGun(aiming);
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
            UpdateMovement();
            base.UpdateAnimator();
            base.UpdatePhysics();
            UpdateIK();

            IK.LeftHand.weight = Mathf.Lerp(start, end, elapsedTime);
            CameraController.Zoom = Mathf.Lerp(start, end, elapsedTime);

            elapsedTime += Time.deltaTime * 3;
            yield return null;
        }
    }

    private IEnumerator ToggleGun(bool aiming)
    {
        elapsedTime = 0;
        while (elapsedTime <= 1)
        {
            if (aiming)
            {
                gun.position = Vector3.Lerp(Player.GunHolster.position, desiredPoint.position, elapsedTime);
                gun.rotation = Quaternion.Lerp(Player.GunHolster.rotation, desiredPoint.rotation, elapsedTime);
            }
            else
            {
                gun.position = Vector3.Lerp(desiredPoint.position, Player.GunHolster.position, elapsedTime);
                gun.rotation = Quaternion.Lerp(desiredPoint.rotation, Player.GunHolster.rotation, elapsedTime);
            }

            UpdateMovement();
            base.UpdateAnimator();
            base.UpdatePhysics();
            UpdateIK();

            elapsedTime += Time.deltaTime * 3;
            yield return null;
        }

        if (aiming)
        {
            gun.position = desiredPoint.position;
            gun.rotation = desiredPoint.rotation;
        }
        else
        {
            gun.position = Player.GunHolster.position;
            gun.rotation = Player.GunHolster.rotation;
        }
    }

    private IEnumerator ThrowHook(GameObject node)
    {
        if (!node)
            yield break;

        InTransition = true;
        anim.SetBool("hook", true);
        
        sword.parent = null;

        desiredDirection = node.transform.position - Player.transform.position;

        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            sword.position = Vector3.Lerp(sword.position, node.transform.position - node.transform.forward * 0.3f, elapsedTime);
            sword.rotation = Quaternion.Lerp(sword.rotation, Quaternion.FromToRotation(Vector3.up, node.transform.forward), elapsedTime);
            elapsedTime += Time.deltaTime;

            base.UpdateMovement();
            base.UpdateAnimator();
            base.UpdateIK();
            base.UpdatePhysics();

            yield return null;
        }
        sword.parent = node.transform.parent;
        yield return HookTravel(node);
    }
    private IEnumerator HookTravel(GameObject node)
    {
        Vector3 startPos = node.transform.parent.position;
        Vector3 offsetPos = Player.transform.position + (Player.transform.up * 1.1f) + (Player.transform.forward * 0.3f);

        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            node.transform.parent.position = Vector3.Lerp(startPos, offsetPos, elapsedTime);
            node.transform.parent.rotation = Quaternion.Lerp(node.transform.parent.rotation, Player.transform.rotation, elapsedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector3.zero;
        stateManager.ChangeState(new CarryState(stateManager, node.GetComponent<CarryNode>(), grounded));
        InTransition = false;
    }

    //State Updates
    protected override void UpdateMovement()
    {
        base.UpdateMovement();

        Player.transform.LookAt(Player.transform.position + lookDirection);
    }
    protected override void UpdateIK()
    {
        base.UpdateIK();
        //Should be replaced by equip animation
        //Hand transform

        IK.LeftHand.position = Player.weapons[0].Grip(0).position;
        IK.LeftHand.rotation = Player.weapons[0].Grip(0).rotation;
    }

    public override void OnTriggerEnter(Collider other) { }
}
