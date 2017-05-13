using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentState : CharacterState
{
    protected static Weapon[] weapons;
    protected static bool[] hasWeapon = { false, false };
    protected static Transform SwordSheath;
    protected static Transform GunHolster;

    protected const int UP = 0;
    protected const int DOWN = 1;
    protected const int STAY = 2;

    protected const int GUN = 0;
    protected const int SWORD = 1;
    public static int rightTriggerState = -1;
    public static int leftTriggerState = -1;

    public EquipmentState() : base(Player)
    {
        if (weapons == null) weapons = Player.transform.GetComponentsInChildren<Weapon>();
        if (!SwordSheath) SwordSheath = GameObject.Find("Player_Sheath").transform;
        if (!GunHolster) GunHolster = GameObject.Find("Player_Holster").transform;

        anim.SetBool("hasSword", false);
        anim.SetBool("aiming", false);
    }

    public override CharacterState UpdateState() { return HandleStateChange(); }

    protected override CharacterState HandleStateChange()
    {
        if (anim.GetBool("climbing"))
            return null;

        if (Input.GetButtonDown("Attack") || rightTriggerState == DOWN)
            return new CombatState();

        if (Input.GetButtonDown("Equip"))
            return new CombatState();

        if (Input.GetButtonDown("Aim") || leftTriggerState == DOWN)
        {
            return new AimState();
        }

        return null;
    }

    public override CharacterState OnTriggerStay(Collider other)
    {
        if (Input.GetAxis("Vertical") > Mathf.Epsilon)
        {
            if (other.gameObject.CompareTag("Pushable"))
                return new PushState(other.gameObject);
        }
        if (other.gameObject.CompareTag("CarryNode") && Input.GetButtonDown("Action"))
        {
            CarryNode temp = other.gameObject.GetComponent<CarryNode>();
            if (temp && temp.Active)
                return new CarryState(temp);
        }
        return null;
    }

    public static void ToggleWeapon(int WeaponType)
    {
        Transform weapon = weapons[WeaponType].transform;
        if (hasWeapon[WeaponType])
        {
            if (WeaponType == SWORD)
                weapon.parent = SwordSheath;
                //weapon.parent = Player.transform.FindChild("char_ethan_skeleton/char_ethan_Hips/char_ethan_Spine/char_ethan_Spine1/Player_Sheath");
            else if (WeaponType == GUN)
                weapon.parent = GunHolster;
                //weapon.parent = Player.transform.FindChild("char_ethan_skeleton/char_ethan_Hips/char_ethan_Spine/char_ethan_LeftUpLeg/Player_Holster");
        }
        else
        {
            if (WeaponType == SWORD)
            {
                weapon.parent = anim.GetBoneTransform(HumanBodyBones.RightHand);
                weapon.localPosition = new Vector3(-0.1f, 0.035f, 0.02f);
                weapon.rotation = weapon.parent.rotation * new Quaternion(1, 0, 0, 1);
            }
            else if (WeaponType == GUN)
            {
                weapon.parent = anim.GetBoneTransform(HumanBodyBones.LeftHand);
                weapon.localPosition = new Vector3(-0.1f, 0.05f, -0.04f);
            }
            else Debug.Log("Something went wrong with weapon :" + weapon.gameObject.name);
        }
        hasWeapon[WeaponType] = !hasWeapon[WeaponType];
        anim.SetBool("hasSword", hasWeapon[SWORD]);
    }

    public static void RightTriggerState()
    {
        if (rightTriggerState == -1)
        {
            if (Input.GetAxisRaw("RightTrigger") > 0)
                rightTriggerState = DOWN;
        }
        else if (rightTriggerState > 0)
        {
            if (Input.GetAxisRaw("RightTrigger") > 0)
                rightTriggerState = STAY;
            else
                rightTriggerState = UP;
        }
        else rightTriggerState = -1;
    }

    public static void LeftTriggerState()
    {
        if (leftTriggerState == -1)
        {
            if (Input.GetAxisRaw("LeftTrigger") > 0)
                leftTriggerState = DOWN;
        }
        else if (leftTriggerState > 0)
        {
            if (Input.GetAxisRaw("LeftTrigger") > 0)
                leftTriggerState = STAY;
            else
                leftTriggerState = UP;
        }
        else leftTriggerState = -1;
    }
}