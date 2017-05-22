using UnityEngine;
using System.Collections;

public class FallImpactAction : PlayerState
{
    float startTime = 0.0f;
    float elapsedtime = 0.0f;

    public FallImpactAction() : base()
    {
        anim.SetBool("isGrounded", true);
        anim.ResetTrigger("falling");
        startTime = Time.deltaTime;
        IK.GlobalWeight = 0f;

        rb.velocity = Vector3.zero;
    }

    public override CharacterState UpdateState()
    {
        
        return HandleStateChange();
    }

    protected override CharacterState HandleStateChange()
    {
        elapsedtime += Time.deltaTime;
        if (elapsedtime - startTime >= 5.0f)
        {
            return new GroundedState();
        }
        else return null;
    }

    public override void UpdateIK()
    {
        anim.SetFloat("Speed", 0);
    }
}
