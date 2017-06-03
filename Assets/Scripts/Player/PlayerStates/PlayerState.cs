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
}