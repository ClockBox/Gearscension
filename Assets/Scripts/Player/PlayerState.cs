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
    private bool inTransition = false;
    protected float elapsedTime;
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
        rb = PlayerController.rb;
        anim = PlayerController.anim;
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
            UpdateMovement();
            UpdateAnimator();
            UpdateIK();

            if (!inTransition)
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
            if (this != stateManager.State)
            {
                Debug.LogWarning("RogueState: " + this + "\tCurrent State:" + stateManager.State);
                stopState = true;
                yield break;
            }
            yield return new WaitForFixedUpdate();
            UpdatePhysics();
        }
    }

    //State Updates
    protected virtual IEnumerator HandleInput() { yield break; }
    protected virtual void UpdateMovement() { }
    protected virtual void UpdateAnimator() { }
    protected virtual void UpdateIK() { }
    protected virtual void UpdatePhysics() { }

    //Trigger Functions
    public virtual void OnTriggerEnter(Collider collider) { }
    public virtual void OnTriggerStay(Collider collider) { }
    public virtual void OnTriggerExit(Collider collider) { }

    //Colission Functions
    public virtual void OnCollisionEnter(Collision collision) { }
    public virtual void OnCollisionStay(Collision collision) { }
    public virtual void OnCollisionExit(Collision collision) { }
}