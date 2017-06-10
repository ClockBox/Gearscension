using UnityEngine;

public class EquipmentState : CharacterState
{
    protected const int UP = 0;
    protected const int DOWN = 1;
    protected const int STAY = 2;

    public static int rightTriggerState = -1;
    public static int leftTriggerState = -1;

    protected static GameObject[] Hooks;
    protected ClimbingNode ClosestHook;
    protected float HookRange = 15;

    public EquipmentState() : base(Player)
    {
        anim.SetBool("hasSword", false);
        anim.SetBool("aiming", false);
    }

    public override CharacterState UpdateState() { return HandleStateChange(); }

    protected override CharacterState HandleStateChange()
    {
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
    protected virtual IKPositionNode FindHookTarget()
    {
        Hooks = GameObject.FindGameObjectsWithTag("HookNode");

        float closestDistance = 0;
        for (int i = 0; i < Hooks.Length; i++)
        {
            Vector3 checkDistance = Hooks[i].transform.position - Player.transform.position;
            if (checkDistance.magnitude < HookRange && Vector3.Dot(lookDirection, Hooks[i].transform.forward) > 0.5f)
            {
                float checkAngle = (Vector3.Dot(Hooks[i].transform.position - Player.transform.position, Camera.main.transform.forward));
                if (checkAngle > closestDistance)                   
                {
                    closestDistance = checkAngle;
                    ClosestHook = Hooks[i].GetComponent<ClimbingNode>();
                }
            }
        }
        return ClosestHook;
    }

    public override CharacterState OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ClimbingNode"))
            return new EquipmentState();
        return null;
    }

    public override CharacterState OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Action"))
        {
            if (other.gameObject.CompareTag("CarryNode"))
            {
                CarryNode node = other.gameObject.GetComponent<CarryNode>();
                if (node && node.Active)
                    return new CarryState(node);
            }
        }
        return null;
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