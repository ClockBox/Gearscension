using UnityEngine;
using System.Collections;

public class DodgeAction : PlayerState
{
    float startTime = 0.0f;
    float elapsedtime = 0.0f;

    Vector3 desiredDirection;

    public DodgeAction(Vector3 direction) : base()
    {
        anim.SetTrigger("roll");

        MovementSpeed = 10f;
        startTime = Time.deltaTime;

        IK.headWeight = 0;
        IK.GlobalWeight = 0f;

        desiredDirection = direction;
    }

    public override CharacterState UpdateState()
    {
        HandleInput();
        
        return HandleStateChange();
    }

    protected override CharacterState HandleStateChange()
    {
        elapsedtime += Time.deltaTime;
        if (elapsedtime - startTime >= 1.3f)
            return new GroundedState();
        else return null;
    }

    protected override void HandleInput()
    {
        base.HandleInput();

        float vel_Y = rb.velocity.y;

        rb.velocity = (desiredDirection.normalized * anim.velocity.magnitude * 2f) + (Player.transform.up * vel_Y);
        rb.AddForce(Player.transform.up * -9.81f * rb.mass);
    }

    public override void UpdateIK()
    {
        anim.SetFloat("Speed", 0);
    }
}