using System.Collections;
using UnityEngine;

public class CombatState : WalkingState
{
    ClimbingNode hookNode;
    Transform sword;

    private bool hooked = false;

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
        Player.StartCoroutine(Player.ToggleWeapon(1, 0.6f, 1.24f));
    }
    public override IEnumerator ExitState()
    {
        yield return base.ExitState();
        if (hooked)
            yield return Player.ToggleWeapon(1, 0.6f, 1.24f);
        else
            Player.StartCoroutine(Player.ToggleWeapon(1, 0.6f, 1.24f));
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.X))
            Player.StartCoroutine(ThrowHook(Player.FindHookTarget()));
        else if (hooked)
        {
            if (Input.GetButtonDown("Jump"))
                grounded = false;

            if (moveDirection.magnitude > 0)
            {
                IK.RightHand.weight = 0;
                inTransition = true;
                Player.weapons[1].transform.parent = anim.GetBoneTransform(HumanBodyBones.RightHand);
                stateManager.ChangeState(new ClimbState(stateManager, hookNode));
            }
        }
        else
        {
            if (Input.GetButtonDown("Equip"))
                stateManager.ChangeState(new UnequipedState(stateManager,grounded));

            else if (Input.GetButtonDown("Attack"))
                yield return Attack();

            else
                yield return base.HandleInput();
        }
    }

    //State Actions
    private IEnumerator Attack()
    {
        anim.SetTrigger("attack");
        (Player.weapons[1] as Sword).Blade.enabled = true;
        yield return new WaitForSeconds(0.5f);
        (Player.weapons[1] as Sword).Blade.enabled = false;
    }
    private IEnumerator ThrowHook(ClimbingNode node)
    {
        if (!node)
            yield break;

        inTransition = true;
        hooked = true;

        anim.SetBool("hook", true);
        sword = Player.weapons[1].transform;
        sword.parent = null;
        hookNode = node;
        desiredDirection = hookNode.transform.position - Player.transform.position;

        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            sword.position = Vector3.Lerp(sword.position, hookNode.rightHand.position - hookNode.transform.forward * 0.5f, elapsedTime);
            sword.rotation = Quaternion.Lerp(sword.rotation, Quaternion.FromToRotation(Vector3.up, hookNode.transform.forward), elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return HookTravel();
    }
    private IEnumerator HookTravel()
    {
        IK.SetIKPositions(Player.weapons[1].Grip(1), hookNode.leftHand, hookNode.rightFoot, hookNode.leftFoot);

        Vector3 temp = desiredDirection;
        temp.y = Player.transform.position.y;
        Player.transform.LookAt(Player.transform.position + temp);

        anim.SetBool("climbing", true);

        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            Player.transform.position = Vector3.Lerp(Player.transform.position, hookNode.transform.position - hookNode.transform.forward * 0.4f - Player.transform.up * 1.5f, elapsedTime);
            IK.RightHand.weight = elapsedTime;
            IK.LeftHand.weight = elapsedTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Player.transform.position = hookNode.PlayerPosition;
        rb.velocity = Vector3.zero;
        inTransition = false;
    }

    //State Updates
    protected override void UpdateMovement()
    {
        if (hooked)
        {
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector3(moveX, moveY, 0);

            //Rotation
            Player.transform.rotation = hookNode.transform.rotation;
            if (hookNode.FreeHang)
                Player.transform.localEulerAngles = new Vector3(0, Player.transform.localEulerAngles.y, Player.transform.localEulerAngles.z);
        }
        else
            base.UpdateMovement();
    }
    protected override void UpdateAnimator()
    {
        if (!hooked)
            base.UpdateAnimator();
        else anim.ResetTrigger("hook");
    }
    protected override void UpdateIK()
    {
        if (!hooked)
            base.UpdateIK();
        else if (anim.GetBool("climbing"))
        {
            IK.SetIKPositions(null, hookNode.leftHand, hookNode.rightFoot, hookNode.leftFoot);
            IK.headWeight = 0;
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
