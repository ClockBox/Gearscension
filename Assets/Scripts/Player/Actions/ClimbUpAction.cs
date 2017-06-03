using System.Collections;
using UnityEngine;

public class ClimbUpAction : PlayerState
{
    float startTime = 0.0f;
    float elapsedtime = 0.0f;
    float waitTime;
    
    ClimbingEdge edge;

    Collider col;

    public ClimbUpAction(ClimbingEdge Edge) : base()
    {
        startTime = Time.deltaTime;

        edge = Edge;

        Player.transform.parent = edge.transform;

        col = Player.GetComponent<Collider>();
        col.enabled = false;

        anim.SetTrigger("climbUp");
        IK.headWeight = 0;

        if (anim.GetBool("braced"))
            waitTime = 1.0f;
        else waitTime = 3.8f;

        rb.velocity = Vector3.zero;
    }

    public override CharacterState UpdateState()
    {
        HandleInput();
        return HandleStateChange();
    }

    protected override void HandleInput()
    {
        Player.transform.position += anim.velocity * Time.deltaTime;
    }

    protected override CharacterState HandleStateChange()
    {
        elapsedtime += Time.deltaTime;
        if (elapsedtime - startTime >= waitTime)
        {
            col.enabled = true;
            return new GroundedState();
        }
        else return null;
    }

    public override IEnumerator ExitState()
    {
        moveDirection = Vector3.zero;
        Player.transform.parent = null;

        Player.transform.LookAt(Player.transform.position + Vector3.ProjectOnPlane(Player.transform.forward, Vector3.up));

        yield return null;
    }
}