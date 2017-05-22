using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState
{
    protected static GameObject Player;
    protected static Rigidbody rb;
    protected static Animator anim;
    protected static IKController IK;

    protected Vector3 lookDirection;

    public CharacterState(GameObject player)
    {
        if (!Player) Player = player;
        if (!rb) rb = Player.GetComponent<Rigidbody>();
        if (!anim) anim = Player.GetComponent<Animator>();
        if (!IK) IK = Player.GetComponent<IKController>();
    }

    public float SignedAngle(Vector3 v1, Vector3 v2, Vector3 normal)
    {
        float angle = Vector3.Angle(v1, v2);
        if (Vector3.Dot(Vector3.Cross(v1, v2),normal) < 0)
            angle = -angle;
        return angle;
    }

    public virtual CharacterState UpdateState() { return null; }
    protected virtual CharacterState HandleStateChange() { return null; }
    public virtual CharacterState OnTriggerEnter(Collider other) { return null; }
    public virtual CharacterState OnTriggerStay(Collider other) { return null; }
    public virtual CharacterState OnTriggerExit(Collider other) { return null; }

    public virtual void ExitState() { }
    protected virtual void HandleInput()
    {
        lookDirection = Camera.main.transform.forward.normalized;
        lookDirection = Vector3.ProjectOnPlane(lookDirection, Player.transform.up);

        if (Vector3.Dot(Player.transform.forward, lookDirection) < 0)
            IK.headWeight = Mathf.MoveTowards(IK.headWeight, 0, 0.05f);
        else

            IK.headWeight = Mathf.MoveTowards(IK.headWeight, 1, 0.05f);
    }
    public virtual void UpdateIK()
    {
        IK.RightHand.weight = anim.GetFloat("RightHandWeight");
        IK.LeftHand.weight = anim.GetFloat("LeftHandWeight");
        IK.RightFoot.weight = anim.GetFloat("RightFootWeight");
        IK.LeftFoot.weight = anim.GetFloat("LeftFootWeight");
    }
}
