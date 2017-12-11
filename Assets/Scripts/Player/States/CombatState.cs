using System.Collections;
using UnityEngine;

public class CombatState : MoveState
{
    private Transform sword;

    public CombatState(StateManager manager, bool grounded) : base(manager, grounded) { }

    //Transitions
    public override IEnumerator EnterState(PlayerState prevState)
    {
        if (prevState as HookState == null)
            yield return ToggleSword(true);
        yield return base.EnterState(prevState);
    }
    public override IEnumerator ExitState(PlayerState nextState)
    {
        yield return base.ExitState(nextState);
        if (nextState as HookState == null)
            yield return ToggleSword(false);
    }

    //State Behaviour
    protected override IEnumerator HandleInput()
    {
        if (Input.GetButtonDown("Attack") || Player.RightTrigger.Down)
            yield return Attack();

        else if (Player.GunUpgrades >= 0 && (Input.GetButton("Aim") || Player.LeftTrigger.Stay))
            stateManager.ChangeState(new AimState(stateManager, grounded));

        if (Input.GetButtonDown("Equip"))
            stateManager.ChangeState(new UnequipedState(stateManager, grounded));
        else
            yield return base.HandleInput();
    }

    //State Actions
    private IEnumerator ToggleSword(bool Equiped)
    {
        float waitTime = 1.0f;
        Player.StartCoroutine(ToggleSword(Equiped, 0.6f, waitTime));
        elapsedTime = 0;
        while (elapsedTime < waitTime)
        {
            if (Input.GetButtonDown("Jump") && grounded)
                    Jump();

            UpdateAnimator();
            UpdateMovement();
            UpdatePhysics();

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
            GameManager.Instance.AudioManager.AudioPlayer = Player.SFX;
            GameManager.Instance.AudioManager.playAudio("sfxswordsheath");

            sword.parent = Player.SwordSheath;
            sword.rotation = Player.SwordSheath.rotation;
            sword.localEulerAngles = new Vector3(0, 0, 90);
            sword.position = Player.SwordSheath.position;
        }
        else
        {
            GameManager.Instance.AudioManager.AudioPlayer = Player.SFX;
            GameManager.Instance.AudioManager.playAudio("sfxsworddraw");

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

    public override void OnTriggerEnter(Collider other)
    {
        if (!InTransition && canClimb)
        {
            if (other.CompareTag("ClimbingNode") || other.CompareTag("HookNode"))
            {
                if (Vector3.Dot(other.transform.forward, Player.transform.forward) > 0)
                {
                    moveDirection = Vector3.zero;
                    stateManager.ChangeState(new HookState(stateManager, other.GetComponent<ClimbingNode>()));
                }
            }
        }
    }
}
