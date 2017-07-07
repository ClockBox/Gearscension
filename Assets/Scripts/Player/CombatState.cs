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
        IK.headWeight = 1;
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

        else if (Input.GetButtonDown("Attack"))
            yield return Attack();

        else if (hooked)
        {
            if (Input.GetButtonDown("Jump"))
            {
                grounded = false;
                canClimb = false;
                IK.headWeight = 1;
            }

            else if (Input.GetButtonDown("Equip"))
                stateManager.ChangeState(new ClimbState(stateManager, hookNode));

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
                stateManager.ChangeState(new UnequipedState(stateManager, grounded));
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
        anim.SetBool("hook", true);

        sword = Player.weapons[1].transform;
        sword.parent = null;

        desiredDirection = node.transform.position - Player.transform.position;

        if (hookNode)
        {
            IK.RightHand.position = hookNode.rightHand.position;
            IK.RightHand.rotation = hookNode.rightHand.rotation;
        }

        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            sword.position = Vector3.Lerp(sword.position, node.rightHand.position - node.transform.forward * 0.5f, elapsedTime);
            sword.rotation = Quaternion.Lerp(sword.rotation, Quaternion.FromToRotation(Vector3.up, node.transform.forward), elapsedTime);
            elapsedTime += Time.deltaTime;

            base.UpdateMovement();
            base.UpdateAnimator();
            base.UpdateIK();
            base.UpdatePhysics();

            yield return null;
        }
        yield return HookTravel(node);
    }
    private IEnumerator HookTravel(ClimbingNode node)
    {
        hooked = true;
        hookNode = node;

        anim.SetBool("climbing", true);

        elapsedTime = 0;
        while (elapsedTime < 1)
        {
            Player.transform.position = Vector3.Lerp(Player.transform.position, node.PlayerPosition, elapsedTime);
            Player.transform.rotation = Quaternion.Lerp(Player.transform.rotation, node.transform.rotation, elapsedTime);

            IK.GlobalWeight = elapsedTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        IK.SetIKPositions(Player.weapons[1].Grip(1), node.leftHand, node.rightFoot, node.leftFoot);
        if (node.FreeHang)
            Player.transform.localEulerAngles = new Vector3(0, Player.transform.localEulerAngles.y, Player.transform.localEulerAngles.z);

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
            if(hookNode.FreeHang)
                anim.SetFloat("braced", 0);
            else
                anim.SetFloat("braced", 1);
        }
    }
    protected override void UpdateIK()
    {
        if (!hooked)
            base.UpdateIK();
        else
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
