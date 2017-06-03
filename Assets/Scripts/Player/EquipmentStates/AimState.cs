using System.Collections;
using UnityEngine;

public class AimState : EquipmentState
{
    float BulletScale = 1;
    bool canShoot;

    public AimState()
    {
        Player.transform.GetChild(0).position -= Player.transform.right * 0.2f + Player.transform.up * 0.2f;
        CameraController.Zoomed = true;
        anim.SetBool("aiming", true);
    }

    public override CharacterState UpdateState()
    {
        Vector3 lookDirection = Player.transform.position + Vector3.ProjectOnPlane(Camera.main.transform.forward, Player.transform.up);
        Player.transform.LookAt(lookDirection);
        HandleInput();
        
        return HandleStateChange();
    }

    protected override void HandleInput()
    {
        if (Input.GetButtonUp("Aim") || leftTriggerState == UP || Input.GetButtonDown("Roll"))
            anim.SetBool("aiming", false);

        if (Input.GetButton("Attack") || Input.GetButtonDown("Attack") || rightTriggerState == DOWN || rightTriggerState == STAY)
            ChargeShot();

        if (Input.GetButtonUp("Attack") || rightTriggerState == UP)
            Shoot();
    }

    protected override CharacterState HandleStateChange()
    {
        if (anim.GetBool("climbing"))
            return new EquipmentState();

        if (IK.LeftHand.weight == 0 && !Input.GetButton("Aim") && leftTriggerState != STAY)
            return new EquipmentState();

        return null;
    }

    public override void UpdateIK()
    {
        base.UpdateIK();

        IK.RightFoot.weight = 0;
        IK.LeftFoot.weight = 0;

        Vector3 DesiredPosition = Player.transform.GetChild(0).position - Player.transform.up * 0.2f + Camera.main.transform.forward * 0.5f;
        Weapon Gun = StateManager.weapons[0];
        if (IK.LeftHand.weight == 1)
        {
            canShoot = true;
            Gun.transform.position = DesiredPosition;
            Gun.transform.rotation = Camera.main.transform.rotation;
        }
        else if(IK.LeftHand.weight != 0)
        {
            canShoot = false;
            Gun.transform.parent = null;
            Gun.transform.position = Vector3.Lerp(StateManager.GunHolster.position, DesiredPosition, IK.LeftHand.weight);
            Gun.transform.rotation = Quaternion.Lerp(StateManager.GunHolster.rotation, Camera.main.transform.rotation, IK.LeftHand.weight);
        }

        IK.LeftHand.position = StateManager.weapons[0].Grip(0).position;
        IK.LeftHand.rotation = StateManager.weapons[0].Grip(0).rotation;
    }

    public override IEnumerator ExitState()
    {
        Player.transform.GetChild(0).position += Player.transform.right * 0.2f + Player.transform.up * 0.2f;
        CameraController.Zoomed = false;
        yield return null; 
    }

    void ChargeShot()
    {
        if (BulletScale < 3)
            BulletScale += Time.deltaTime / 2;
    }

    void Shoot()
    {
        if (canShoot)
        {
            StateManager.weapons[0].SendMessage("Shoot", BulletScale);
            BulletScale = 1;
        }
    }

    public override CharacterState OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ClimbingNode"))
            return new EquipmentState();
        return null;
    }
    public override CharacterState OnTriggerStay(Collider other) { return null; }
}
