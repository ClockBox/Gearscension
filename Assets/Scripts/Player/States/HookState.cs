using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookState : ClimbState
{
    private Sword sword;

    public HookState(StateManager manager, ClimbingNode node) : base(manager, node) { }

    //Transitions
    public override IEnumerator EnterState(PlayerState prevState)
    {
        anim.ResetTrigger("hook");
        yield return base.EnterState(prevState);

        sword = (Sword)Player.weapons[1];

        if ((prevState as ClimbState) != null)
            IK.RightHand.weight = 0;
        else
        {
            IK.RightHand.position = sword.Grip(0).position;
            IK.RightHand.rotation = sword.Grip(0).rotation;
            IK.GlobalWeight = 1;
        }

        if (sword.transform.parent == Player.SwordSheath)
            yield return ToggleSword(true);
    }
    public override IEnumerator ExitState(PlayerState nextState)
    {
        if (Player.selectedHook)
        {
            Light tempLight;
            if(tempLight = Player.selectedHook.transform.parent.GetChild(0).GetComponent<Light>())
                tempLight.enabled = false;
            Player.selectedHook = null;
        }
        if (nextState as CombatState == null)
            yield return ToggleSword(false);
        yield return base.ExitState(nextState);
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (Input.GetButton("Hook"))
            Player.FindHookTarget();
        else if (Input.GetButtonUp("Hook"))
            Player.StartCoroutine(ThrowHook(Player.selectedHook));

        if (Input.GetButtonDown("Attack") || Player.RightTrigger.Down)
            yield return Attack();

        else if (Input.GetButtonDown("Jump"))
            Jump();

        else if ((!currentNodes[0] && !currentNodes[1])
            || (!currentNodes[0].Active && !currentNodes[1].Active)
            || (!currentNodes[0].gameObject.activeInHierarchy && !currentNodes[1].gameObject.activeInHierarchy))
        {
            InTransition = true;
            stateManager.ChangeState(new UnequipedState(stateManager, false));
        }

        else if (Input.GetButtonDown("Equip"))
        {
            IK.RightHand.weight = 0;
            InTransition = true;
            Player.weapons[1].transform.parent = anim.GetBoneTransform(HumanBodyBones.RightHand);

            stateManager.ChangeState(new ClimbState(stateManager, currentNodes[0]));
        }

        else if (moveDirection.magnitude > 0)
        {
            IK.RightHand.weight = 0;
            InTransition = true;
            Player.weapons[1].transform.parent = anim.GetBoneTransform(HumanBodyBones.RightHand);

            stateManager.ChangeState(new ClimbState(stateManager, currentNodes[0]));
        }
    }

    //State Actions
    private void Jump()
    {
        if (currentNodes[0])
        {
            anim.SetBool("climbing", false);
            anim.SetBool("isGrounded", false);

            //Wall Eject
            if (Vector3.Dot(currentNodes[0].transform.transform.forward, lookDirection) < 0)
            {
                Player.transform.LookAt(Player.transform.position + lookDirection);
                rb.velocity = Vector3.ProjectOnPlane(lookDirection.normalized, Vector3.up) * 2.5f + Player.transform.up * 2;
            }
            //Drop/Move allong wall
            else
                rb.velocity = (Player.transform.right * moveX / 2 + Vector3.up * moveY) * 5 + Vector3.up;

            Player.transform.localEulerAngles = new Vector3(0, Player.transform.localEulerAngles.y, Player.transform.localEulerAngles.z);
            canClimb = false;
            stateManager.ChangeState(new CombatState(stateManager, false));
        }
    }

    private IEnumerator ToggleSword(bool Equiped)
    {
        float waitTime = 1.3f;
        Player.StartCoroutine(ToggleSword(Equiped, 0.6f, waitTime));
        elapsedTime = 0;
        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    public IEnumerator ToggleSword(bool Equiped, float toggleTime, float transitionTime)
    {
        sword = (Sword)Player.weapons[1];
        anim.SetBool("hasSword", Equiped);
        anim.SetBool("aiming", false);
        IK.RightHand.weight = 0;

        yield return new WaitForSeconds(toggleTime);

        if (!Equiped)
        {
            sword.transform.parent = Player.SwordSheath;
            sword.transform.rotation = Player.SwordSheath.rotation;
            sword.transform.localEulerAngles = new Vector3(0, 0, 90);
            sword.transform.position = Player.SwordSheath.position;
        }
        else
        {
            sword.transform.parent = anim.GetBoneTransform(HumanBodyBones.RightHand);
            sword.transform.localPosition = new Vector3(0.1f, 0.02f, 0);
            sword.transform.localEulerAngles = new Vector3(90, 0, -90);
        }
        yield return new WaitForSeconds(transitionTime - toggleTime);
    }

    private IEnumerator Attack()
    {
        anim.SetTrigger("attack");
        (Player.weapons[1] as Sword).Blade.enabled = true;

        elapsedTime = 0;
        while (elapsedTime < 0.5f)
        {
            if ((!currentNodes[0] && !currentNodes[1])
            || (!currentNodes[0].Active && !currentNodes[1].Active)
            || (!currentNodes[0].gameObject.activeInHierarchy && !currentNodes[1].gameObject.activeInHierarchy))
                break;
            UpdateIK();
            UpdateMovement();
            UpdateAnimator();

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        (Player.weapons[1] as Sword).Blade.enabled = false;
    }

    //State Updates
    protected override void UpdateMovement()
    {
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        base.UpdateMovement();
    }

    protected override void UpdateIK()
    {
        IK.SetIKPositions(currentNodes[0].rightHand, currentNodes[0].leftHand, currentNodes[0].rightFoot, currentNodes[0].leftFoot);
        IK.RightElbow = Player.transform.position + Player.transform.right + Player.transform.up;

        if (!sword) sword = (Sword)Player.weapons[1];

        if (sword.transform.parent != anim.GetBoneTransform(HumanBodyBones.RightHand))
        {
            IK.RightHand.Set(sword.Grip(0).transform);
            IK.RightHand.weight = 1;
        }
        else IK.RightHand.weight = 0;
    }

    #region Hook Functions
    protected IEnumerator ThrowHook(GameObject node)
    {
        if (!node)
            yield break;

        InTransition = true;
        anim.SetBool("hook", true);

        Transform sword = Player.weapons[1].transform;
        sword.parent = null;

        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            sword.position = Vector3.Lerp(sword.position, node.transform.position - node.transform.forward * 0.3f + node.transform.right * 0.1f, elapsedTime);
            sword.rotation = Quaternion.Lerp(sword.rotation, node.transform.rotation * new Quaternion(0, -1, 0, 1), elapsedTime);
            elapsedTime += Time.deltaTime;
            
            base.UpdateAnimator();
            base.UpdateIK();

            yield return null;
        }
        sword.parent = node.transform;

        HandNode pullObject;
        if (pullObject = node.GetComponentInParent<HandNode>())
            yield return HookPull(pullObject);
        else
            yield return HookTravel(node.GetComponent<ClimbingNode>());
    }
    protected IEnumerator HookTravel(ClimbingNode hook)
    {
        if (hook)
        {
            IK.RightHand.position = hook.rightHand.position;
            IK.RightHand.rotation = hook.rightHand.rotation;
        }

        anim.SetBool("climbing", true);

        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            Player.transform.position = Vector3.Lerp(Player.transform.position, hook.PlayerPosition, elapsedTime);
            Player.transform.rotation = Quaternion.Lerp(Player.transform.rotation, hook.transform.rotation, elapsedTime);

            IK.GlobalWeight = elapsedTime;
            IK.SetIKPositions(Player.weapons[1].transform, hook.leftHand, hook.rightFoot, hook.leftFoot);
            IK.RightFoot.weight = 0;
            IK.LeftFoot.weight = 0;
            IK.HeadWeight = 0;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (hook.FreeHang)
            Player.transform.localEulerAngles = new Vector3(0, Player.transform.localEulerAngles.y, Player.transform.localEulerAngles.z);

        rb.velocity = Vector3.zero;
        stateManager.ChangeState(new HookState(stateManager, hook));
        InTransition = false;
    }

    protected IEnumerator HookPull(HandNode pulledObject)
    {
        Vector3 startPos = pulledObject.transform.position;
        Vector3 offsetPos = Player.transform.position + (Player.transform.up * 1.1f) + (Player.transform.forward * 0.3f);

        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            pulledObject.transform.position = Vector3.Lerp(startPos, offsetPos, elapsedTime);
            pulledObject.transform.rotation = Quaternion.Lerp(pulledObject.transform.rotation, Player.transform.rotation, elapsedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        InTransition = false;
    }
    #endregion
}
