using System.Collections; 
using UnityEngine;

public class HookState : EquipmentState
{
    float startTime = 0.0f;
    float elapsedtime = 0.0f;

    float speed = 15;

    Vector3 desiredDirection;

    public HookState(Vector3 hookPos) : base()
    {
        desiredDirection = hookPos - Player.transform.position;
        Vector3 temp = desiredDirection;
        temp.y = 0;
        Player.transform.LookAt(Player.transform.position + temp);

        anim.SetBool("hook",true);
        anim.SetBool("hasSword", true);
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
        if (elapsedtime - startTime >= 1.5f + desiredDirection.magnitude/speed)
            return new EquipmentState();
        return null;
    }

    public override IEnumerator ExitState()
    {
        anim.SetBool("hook",false);
        rb.velocity = Vector3.zero;
        yield return null;
    }

    protected override void HandleInput()
    {
        base.HandleInput();
        if (elapsedtime > 1.5f)
        {
            rb.velocity = (desiredDirection.normalized * speed);
        }
    }
}
