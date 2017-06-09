using UnityEngine;

public class PlayerState : CharacterState
{
    protected static bool Grounded;
    protected static float MovementSpeed = 5.0f;
    protected static float jumpForce = 6f;

    protected static float X = 0.0f;
    protected static float Z = 0.0f;

    public static bool canRun = true;
    public static bool canSprint = true;
    public static bool canClimb = true;

    public PlayerState() : base(Player) { }

    protected override void HandleInput()
    {
        base.HandleInput();

        X = Input.GetAxis("Horizontal");
        Z = Input.GetAxis("Vertical");
    }

    public override CharacterState OnTriggerStay(Collider other)
    {
        if (Input.GetButton("Action"))
        {
            if (other.gameObject.CompareTag("Pushable"))
            {
                if (Input.GetAxis("Vertical") > Mathf.Epsilon && Input.GetAxis("Horizontal") == 0)
                    return new PushState(other.gameObject);
            }
        }
        return null;
    }
}