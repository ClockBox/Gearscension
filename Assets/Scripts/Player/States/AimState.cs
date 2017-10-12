using System;
using System.Collections;
using UnityEngine;

public class AimState : WalkingState
{
    float equipTime = 1.233f;
    float bulletScale = 1;
    float handToGunWeight;
    
    private Transform sword;

    public AimState(StateManager manager,bool grounded) : base(manager,grounded) { }

    //Transitions
    public override IEnumerator EnterState()
    {
        yield return base.EnterState();
        lookDirection = Camera.main.transform.forward;
        lookDirection = Vector3.ProjectOnPlane(lookDirection, Player.transform.up);

        IK.HeadTrunSpeed = 5;

        Player.StartCoroutine(Player.ToggleWeapon(0, 0, 1));
        Player.transform.GetChild(0).position -= Player.transform.right * 0.23f + Player.transform.up * 0.15f;
        yield return ToggleGun();
    }
    public override IEnumerator ExitState()
    {
        yield return base.ExitState();

        IK.HeadTrunSpeed = 1;

        Player.StartCoroutine(Player.ToggleWeapon(0, 1, 1));
        Player.transform.GetChild(0).position += Player.transform.right * 0.23f + Player.transform.up * 0.15f;
        yield return ToggleGun();
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
    IEnumerator ToggleGun()
    {
        int start = !anim.GetBool("aiming") ? 1 : 0;
        int end = anim.GetBool("aiming") ? 1 : 0;

        IK.LeftHand.weight = end;

        //Should be replaced by equip animation
        elapsedTime = 0;
        while (elapsedTime <= equipTime)
        {
            base.UpdateMovement();
            base.UpdateAnimator();
            base.UpdatePhysics();
            base.UpdateIK();

            CameraController.Zoom = Mathf.Lerp(start, end, elapsedTime * 2);
            handToGunWeight = Mathf.Lerp(start, end, elapsedTime * 2);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ThrowHook(GameObject node)
    {
        if (!node)
            yield break;

        InTransition = true;
        anim.SetBool("hook", true);

        sword = Player.weapons[1].transform;
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
       
        //Should be replaced by equip animation
        Vector3 desiredPoint = Player.transform.GetChild(0).position - Player.transform.up * 0.15f + Camera.main.transform.forward * 0.5f;

        //Gun transform
        Player.weapons[0].transform.position = Vector3.Lerp(Player.weapons[0].transform.position, desiredPoint, handToGunWeight);
        Player.weapons[0].transform.rotation = Quaternion.Lerp(Player.weapons[0].transform.rotation, Camera.main.transform.rotation, handToGunWeight);
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
