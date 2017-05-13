using UnityEngine;

public class CombatState : EquipmentState
{
    Transform targetEnemy;
    Transform targetPos;
    Sword sword;

    public CombatState() : base()
    {
        anim.SetBool("hasSword", true);
        anim.SetBool("aiming", false);
        targetPos = new GameObject().transform;
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
        targetPos.position = Player.transform.position + Player.transform.forward * 0.5f + Player.transform.up * 1.5f;
    }

    protected override CharacterState HandleStateChange()
    {
        if (Input.GetButtonDown("Aim") || leftTriggerState == DOWN)
            return new AimState();

        if (Input.GetButtonDown("Equip"))
            return new EquipmentState();

        return null;
    }

    public override void UpdateIK()
    {
        IK.RightHand.weight = 0.8f;
        IK.RightHand.position = targetPos.position;
        IK.RightHand.rotation = targetPos.rotation;
    }

    void Attack()
    {
        anim.SetTrigger("attack");

        sword = weapons[SWORD] as Sword;
        sword.Blade.enabled = true;
    }

    public override void ExitState()
    {
        GameObject.Destroy(targetPos);
    }

    public override CharacterState OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ClimbingNode"))
            return new EquipmentState();
        return null;
    }
    public override CharacterState OnTriggerStay(Collider other) { return null; }
}