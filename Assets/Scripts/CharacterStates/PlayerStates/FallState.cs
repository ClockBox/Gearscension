using UnityEngine;

public class FallState : PlayerState
{
    float elapsedTime = 0.0f;

    bool freeFall = false;

    public FallState() : base()
    {
        anim.SetBool("isGrounded", false);
        anim.ResetTrigger("jump");
        anim.SetFloat("Z", -1);
    }
	
	public override CharacterState UpdateState()
    {
        
        HandleInput();
        return HandleStateChange();
    }

    protected override CharacterState HandleStateChange()
    {
        if (Physics.Raycast(Player.transform.position + (Player.transform.up * 0.5f) - (Player.transform.forward * 0.3f) - (Player.transform.right * 0.3f), -Player.transform.up, 0.6f, IgnoreMechanics) ||
            Physics.Raycast(Player.transform.position + (Player.transform.up * 0.5f) + (Player.transform.forward * 0.3f) + (Player.transform.right * 0.3f), -Player.transform.up, 0.6f, IgnoreMechanics)) 
        {
            if (freeFall)
                return new FallImpactAction();
            else return new GroundedState();
        }
        return null;
    }

    protected override void HandleInput()
    {
        base.HandleInput();

        if (Input.GetButtonDown("Jump") && !canClimb)
            canClimb = true;

        elapsedTime += Time.deltaTime;
        if (Timer(elapsedTime, 3.0f))
        {
            freeFall = true;
            anim.SetTrigger("falling");
        }
        rb.AddForce(Player.transform.up * -9.81f * rb.mass);
    }

    bool Timer(float start, float waitTime)
    {
        if (elapsedTime >= waitTime)
            return true;
        else return false;
    }

    public override CharacterState OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ClimbingNode") && canClimb)
        {
            return new ClimbState(other.gameObject.GetComponent<ClimbingNode>());
        }
        return null;
    }
}
