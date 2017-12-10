using System.Collections;
using UnityEngine;

public class PlayerState
{
    protected PlayerController Player;
    protected StateManager stateManager;
    protected IKController IK;
    protected Rigidbody rb;
    protected Animator anim;

    protected static bool canClimb = true;
    public static bool grounded = true;

    private bool stopState = false;
    protected float elapsedTime;
    private bool inTransition = false;
    public bool InTransition
    {
        get {return inTransition; }
        set {inTransition = value; }
    }

    //Constructor
    public PlayerState(StateManager manager)
    {
        Player = GameManager.Player;
        stateManager = PlayerController.StateM;
        IK = PlayerController.IK;
        rb = PlayerController.RB;
        anim = PlayerController.Anim;
        elapsedTime = 0;
    }
    
    //Transitions
    public virtual IEnumerator EnterState(PlayerState prevState)
    {
        yield return null;
        stateManager.StartState(this);
    }
    public virtual IEnumerator ExitState(PlayerState nextState)
    {
        stopState = true;
        yield return null;
    }
    public void StopState()
    {
        stopState = false;
    }

    //State Loops
    public virtual IEnumerator Update()
    {
        while (!stopState)
        {
            while (GameManager.Instance.Pause)
                yield return null;

            if (this != stateManager.State)
            {
                Debug.LogWarning("RogueState: " + this + "\t\tCurrent State:" + stateManager.State);
                stopState = true;
                yield break;
            }

            if (!Player.Paused)
            {
                UpdateMovement();
                UpdateAnimator();
                UpdateIK();
            }
            else UpdatePaused();

            if (!inTransition || Player.Paused)
                yield return stateManager.StartCoroutine(HandleInput());
            else
                yield return null;

            elapsedTime += Time.deltaTime;
        }
        stateManager.StopCoroutine(HandleInput());
    }

    public virtual IEnumerator FixedUpdate()
    {
        while (!stopState)
        {
            while (GameManager.Instance.Pause)
                yield return null;

            if (this != stateManager.State)
            {
                Debug.LogWarning("RogueState: " + this + "\t\tCurrent State:" + stateManager.State);
                stopState = true;
                yield break;
            }
            yield return new WaitForFixedUpdate();
            if (!Player.Paused)
                UpdatePhysics();
        }
    }

    //State Updates
    protected virtual IEnumerator HandleInput() { yield break; }
    protected virtual void UpdateMovement() { }
    protected virtual void UpdateAnimator() { }
    protected virtual void UpdateIK() { }
    protected virtual void UpdatePhysics() { }

    protected virtual void UpdatePaused()
    {
        anim.SetFloat("Speed", 0);
        rb.velocity = Vector3.zero;
    }

    //Trigger Functions
    public virtual void OnTriggerEnter(Collider collider) { }
    public virtual void OnTriggerStay(Collider collider) { }
    public virtual void OnTriggerExit(Collider collider) { }

    //Colission Functions
    public virtual void OnCollisionEnter(Collision collision) { }
    public virtual void OnCollisionStay(Collision collision) { }
    public virtual void OnCollisionExit(Collision collision) { }
}