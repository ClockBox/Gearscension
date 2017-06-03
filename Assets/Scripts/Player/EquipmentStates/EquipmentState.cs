using UnityEngine;

public class EquipmentState : CharacterState
{
    protected const int UP = 0;
    protected const int DOWN = 1;
    protected const int STAY = 2;

    public static int rightTriggerState = -1;
    public static int leftTriggerState = -1;

    protected static GameObject[] Hooks;
    protected Vector3 ClosestHook = Vector3.zero;
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
    protected virtual Vector3 FindHookTarget()
    {
        Hooks = GameObject.FindGameObjectsWithTag("HookNode");
        ClosestHook = Vector3.zero;

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
                    Debug.Log(Hooks[i].gameObject.name);
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