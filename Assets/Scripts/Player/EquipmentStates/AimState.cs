using UnityEngine;
using System.Collections;

public class AimState : EquipmentState
{
    float BulletScale = 1;
    bool canShoot;

    public AimState() : base()
    {
        Player.transform.GetChild(0).position -= Player.transform.right * 0.2f;
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
        //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit))
        //{
        //    Debug.Log(hit.collider.gameObject.name);
        //}

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

        if (IK.RightHand.weight == 0 && !Input.GetButton("Aim") && leftTriggerState != STAY)
            return new EquipmentState();
        return null;
    }

    public override void UpdateIK()
    {
        base.UpdateIK();

        IK.RightFoot.weight = 0;
        IK.LeftFoot.weight = 0;

        Vector3 DesiredPosition = Player.transform.GetChild(0).position - Player.transform.up * 0.2f + Camera.main.transform.forward * 0.5f;
        if (IK.LeftHand.weight == 1)
        {
            canShoot = true;
            weapons[GUN].transform.position = DesiredPosition;
            weapons[GUN].transform.rotation = Camera.main.transform.rotation;
        }
        else if(IK.LeftHand.weight != 0)
        {
            canShoot = false;
            weapons[GUN].transform.parent = null;
            weapons[GUN].transform.position = Vector3.Lerp(GunHolster.position, DesiredPosition, IK.LeftHand.weight);
            weapons[GUN].transform.rotation = Quaternion.Lerp(GunHolster.rotation, Camera.main.transform.rotation, IK.LeftHand.weight);
        }

        IK.LeftHand.position = weapons[GUN].Grip(0).position;
        IK.LeftHand.rotation = weapons[GUN].Grip(0).rotation;

        IK.RightHand.position = weapons[GUN].Grip(1).position;
        IK.RightHand.rotation = weapons[GUN].Grip(1).rotation;
    }

    public override void ExitState()
    {
        Player.transform.GetChild(0).position += Player.transform.right * 0.2f;
        CameraController.Zoomed = false;
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
            weapons[GUN].SendMessage("Shoot", BulletScale);
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
