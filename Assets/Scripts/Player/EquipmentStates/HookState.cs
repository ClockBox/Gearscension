 using UnityEngine;

public class HookState : EquipmentState
{
    float startTime = 0.0f;
    float elapsedtime = 0.0f;

    float speed = 15;

    Vector3 desiredDirection;

    public HookState(Vector3 hookPos)
    {
        desiredDirection = hookPos - Player.transform.position;
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
        if (elapsedtime - startTime >= desiredDirection.magnitude/speed)
            return new EquipmentState();
        return null;
    }

    protected override void HandleInput()
    {
        base.HandleInput();
        rb.velocity = (desiredDirection.normalized * speed);
    }
}
