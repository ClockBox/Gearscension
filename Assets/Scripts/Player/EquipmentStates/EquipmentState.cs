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

    protected static HookNode[] Hooks;
    protected Vector3 ClosestHook = Vector3.zero;
    protected float HookRange = 15;

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
        if (Input.GetKeyDown(KeyCode.X) && FindHookTarget() != Vector3.zero)
            return new HookState(ClosestHook);

        if (Input.GetButtonDown("Attack") || rightTriggerState == DOWN)
            return new CombatState();

        if (Input.GetButtonDown("Equip"))
            return new CombatState();

        if (anim.GetBool("climbing"))
            return null;

        if (Input.GetButtonDown("Aim") || leftTriggerState == DOWN)
            return new AimState();

        return null;
    }
    protected virtual Vector3 FindHookTarget()
    {
        Hooks = Object.FindObjectsOfType<HookNode>();
        ClosestHook = Vector3.zero;

        float closestDistance = 0;
        for (int i = 0; i < Hooks.Length; i++)
        {
            Vector3 checkDistance = Hooks[i].transform.position - Player.transform.position;
            if (checkDistance.magnitude < HookRange)
            {
                float checkAngle = (Vector3.Dot(Hooks[i].transform.position - Player.transform.position, Camera.main.transform.forward));
                if (checkAngle > closestDistance)                   
                {
                    Debug.Log(Hooks[i].gameObject.name + ":" + checkAngle);
                    closestDistance = checkAngle;
                    ClosestHook = Hooks[i].transform.position - Vector3.up;
                }
            }
        }
        return ClosestHook;
    }

    public override CharacterState OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Pushable"))
        {
            if (Input.GetAxis("Vertical") > Mathf.Epsilon && Input.GetAxis("Horizontal") == 0)
                return new PushState(other.gameObject);
        }
        else if (other.gameObject.CompareTag("CarryNode") && Input.GetButtonDown("Action"))
        {
            CarryNode node = other.gameObject.GetComponent<CarryNode>();
            if (node && node.Active)
                return new CarryState(node);
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
            else if (WeaponType == GUN)
                weapon.parent = GunHolster;
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

    public static void DropWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.GetComponent<BoxCollider>().enabled = true;
            weapons[i].gameObject.AddComponent<Rigidbody>();
            weapons[i].transform.parent = null;
        }
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