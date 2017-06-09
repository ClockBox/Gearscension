using System.Collections; 
using UnityEngine;

public class HookState : EquipmentState
{
    float startTime = 0.0f;
    float elapsedtime = 0.0f;

    float speed = 15;

    Vector3 desiredDirection;
    ClimbingNode Hook;

    public HookState(ClimbingNode Hook) : base()
    {
        this.Hook = Hook;
        desiredDirection = Hook.transform.position - Player.transform.position;

        anim.SetBool("hook",true);
        anim.SetBool("hasSword", true);

        IK.RightHand.weight = 0;
    }

    public override CharacterState UpdateState()
    {
        HandleInput();
        return HandleStateChange();
    }

    protected override CharacterState HandleStateChange()
    {
        if (Input.GetButtonDown("Jump"))
            return new EquipmentState();

        elapsedtime += Time.deltaTime;
        if (elapsedtime - startTime >= 0.8f + desiredDirection.magnitude/speed)
            return new EquipmentState();
        return null;
    }

    public override IEnumerator ExitState()
    {
        Manager.weapons[1].transform.parent = anim.GetBoneTransform(HumanBodyBones.RightHand);
        anim.SetBool("hook",false);
        rb.velocity = Vector3.zero;
        yield return null;
    }

    protected override void HandleInput()
    {
        Transform sword = Manager.weapons[1].transform;

        base.HandleInput();
        if (elapsedtime < 0.1)
            sword.parent = null;
        else if (elapsedtime < 1f)
        {
            sword.position = Vector3.Lerp(sword.position, Hook.rightHand.position - Hook.transform.forward * 0.5f, elapsedtime);
            sword.rotation = Quaternion.Lerp(sword.rotation, Quaternion.FromToRotation(Vector3.up, Hook.transform.forward), elapsedtime);
        }
        else if (elapsedtime > 1f)
        {
            Vector3 temp = desiredDirection;
            temp.y = 0;
            Player.transform.LookAt(Player.transform.position + temp);
            Player.transform.position = Vector3.Lerp(Player.transform.position, Hook.transform.position - Vector3.up * 1f, elapsedtime - 1);

            IK.SetInitialIKPositions(Manager.weapons[1].Grip(1), Hook.leftHand, Hook.rightFoot, Hook.leftFoot);  
        }
    }

    public override void UpdateIK()
    {
        IK.RightHand.weight = Mathf.Lerp(0, 1, Mathf.Pow(elapsedtime - 1, 4));
        IK.LeftHand.weight = Mathf.Lerp(0, 1, Mathf.Pow(elapsedtime - 1, 4));
        IK.RightFoot.weight = Mathf.Lerp(0, 1, Mathf.Pow(elapsedtime - 1, 4));
        IK.LeftFoot.weight = Mathf.Lerp(0, 1, Mathf.Pow(elapsedtime - 1, 4));
    }

    public override CharacterState OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Hook.gameObject)
            return new EquipmentState();
        return null;
    }
}
