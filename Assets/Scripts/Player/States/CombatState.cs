using System.Collections;
using UnityEngine;

public class CombatState : MoveState
{
    private Transform sword;

    public CombatState(StateManager manager, bool grounded) : base(manager, grounded) { }
    public CombatState(StateManager manager, ClimbingNode node) : base(manager, false)
    {
        hookNode = node;
        hooked = true;
    }

    //Transitions
    public override IEnumerator EnterState()
    {
        if (hooked)
        {
            anim.SetBool("climbing", true);
            anim.SetFloat("braced", 1);

            IK.GlobalWeight = 1;

            sword = Player.weapons[1].transform;
            desiredDirection = hookNode.transform.position - Player.transform.position;
        }

        yield return base.EnterState();
        yield return ToggleSword(true);
    }
    public override IEnumerator ExitState()
    {
        yield return ToggleSword(false);
        yield return base.ExitState();
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (Input.GetButtonDown("Attack") || Player.RightTrigger.Down)
            yield return Attack();

        //else if (Player.GunUpgrades >= 0 && (Input.GetButton("Aim") || Player.LeftTrigger.Stay))
            //stateManager.ChangeState(new AimState(stateManager, grounded));

        else if (hooked)
        {
            if (Input.GetButtonDown("Jump"))
                Drop();

            else if (Input.GetButtonDown("Equip"))
            {
                IK.RightHand.weight = 0;
                InTransition = true;
                Player.weapons[1].transform.parent = anim.GetBoneTransform(HumanBodyBones.RightHand);

                ClimbState temp = new ClimbState(stateManager, hookNode);
                temp.transition = false;
                stateManager.ChangeState(temp);
            }

            else if (moveDirection.magnitude > 0)
            {
                IK.RightHand.weight = 0;
                InTransition = true;
                Player.weapons[1].transform.parent = anim.GetBoneTransform(HumanBodyBones.RightHand);

                ClimbState temp = new ClimbState(stateManager, hookNode);
                temp.transition = false;
                stateManager.ChangeState(temp);
            }
        }
        else
        {
            if (Input.GetButtonDown("Equip"))
                stateManager.ChangeState(new UnequipedState(stateManager, grounded));
            else
                yield return base.HandleInput();
        }
    }

    //State Actions
    private IEnumerator ToggleSword(bool Equiped)
    {
        float waitTime = 1.25f;
        Player.StartCoroutine(ToggleSword(Equiped, 0.6f, waitTime));
        elapsedTime = 0;
        while (elapsedTime < waitTime)
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (grounded)
                    Jump();
                else if (hooked)
                    Drop();
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    public IEnumerator ToggleSword(bool Equiped,float toggleTime, float transitionTime)
    {
        sword = Player.weapons[1].transform;
        anim.SetBool("hasSword", Equiped);
        anim.SetBool("aiming", false);
        IK.RightHand.weight = 0;

        yield return new WaitForSeconds(toggleTime);
        
        if (!Equiped)
        {
            sword.parent = Player.SwordSheath;
            sword.rotation = Player.SwordSheath.rotation;
            sword.localEulerAngles = new Vector3(0, 0, 90);
            sword.position = Player.SwordSheath.position;
        }
        else
        {
            sword.parent = anim.GetBoneTransform(HumanBodyBones.RightHand);
            sword.localPosition = new Vector3(0.1f, 0.02f, 0);
            sword.localEulerAngles = new Vector3(90, 0, -90);
        }
        yield return new WaitForSeconds(transitionTime - toggleTime);
    }

    private void Drop()
    {
        grounded = false;
        canClimb = false;
        IK.HeadWeight = 1;
    }

    private IEnumerator Attack()
    {
        anim.SetTrigger("attack");
        (Player.weapons[1] as Sword).Blade.enabled = true;

        elapsedTime = 0;
        while (elapsedTime < 0.5f)
        {
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
        if (hooked)
        {
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector3(moveX, moveY, 0);

            Player.transform.position = hookNode.PlayerPosition;
            Player.transform.rotation = hookNode.transform.rotation;
        }
        else
            base.UpdateMovement();
    }
    protected override void UpdateAnimator()
    {
        if (!hooked)
            base.UpdateAnimator();
        else
        {
            anim.ResetTrigger("hook");
            anim.SetBool("climbing", true);
            if (hookNode.FreeHang)
                anim.SetFloat("braced", Mathf.MoveTowards(anim.GetFloat("braced"), 0, 5 * Time.deltaTime));
            else
                anim.SetFloat("braced", Mathf.MoveTowards(anim.GetFloat("braced"), 1, 5 * Time.deltaTime));
        }
    }
    protected override void UpdateIK()
    {
        if (!hooked)
            base.UpdateIK();
        else
        {
            IK.SetIKPositions(Player.weapons[1].transform, hookNode.leftHand, hookNode.rightFoot, hookNode.leftFoot);
            if (!hookNode.FreeHang)
            {
                IK.RightFoot.weight = Mathf.MoveTowards(IK.RightFoot.weight, 1, 3 * Time.deltaTime);
                IK.LeftFoot.weight = IK.RightFoot.weight;
            }
            IK.HeadWeight = 0;
        }
    }

    protected override void UpdatePhysics()
    {
        if (!hooked)
            base.UpdatePhysics();
        else rb.velocity = Vector3.zero;
    }

    //Trigger Functinos
    public override void OnTriggerEnter(Collider other)
    {
        if (!hooked)
            base.OnTriggerEnter(other);
    }
}
