using UnityEngine;

public class CombatState : EquipmentState
{
    Transform targetEnemy;
    Sword sword;

    public CombatState() : base()
    {
        anim.SetBool("hasSword", true);
    }

    public override CharacterState UpdateState()
    {
        HandleInput();
        return HandleStateChange();
    }

    protected override void HandleInput()
    {
        if (Input.GetButtonDown("Attack") || rightTriggerState == DOWN)
            Attack();
    }

    protected override CharacterState HandleStateChange()
    {
        if (Input.GetKeyDown(KeyCode.X) && FindHookTarget())
            return new HookState(ClosestHook);

        if (Input.GetButtonDown("Aim") || leftTriggerState == DOWN)
            return new AimState();

        if (Input.GetButtonDown("Equip"))
            return new EquipmentState();

        return null;
    }

    void Attack()
    {
        anim.SetTrigger("attack");

        sword = Manager.weapons[1] as Sword;
        sword.Blade.enabled = true;
    }

    public override CharacterState OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ClimbingNode"))
            return new EquipmentState();
        return null;
    }
    public override CharacterState OnTriggerStay(Collider other) { return null; }
}