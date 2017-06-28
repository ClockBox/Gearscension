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

    protected float elapsedTime;
    protected bool stopState = false;
    protected bool inTransition = false;
    public bool InTransition
    {
        get {return inTransition; }
        set {inTransition = value; }
    }

    //Constructor
    public PlayerState(StateManager manager)
    {
        Player = PlayerController.Player;
        stateManager = PlayerController.StateM;
        IK = PlayerController.IK;
        rb = PlayerController.rb;
        anim = PlayerController.anim;
        elapsedTime = 0;
    }
    
    //Transitions
    public virtual IEnumerator EnterState()
    {
        yield return null;
        stateManager.StartState(this);
    }
    public virtual IEnumerator ExitState()
    {
        stopState = true;
        yield return null;
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
    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void OnTriggerStay(Collider other) { }
    public virtual void OnTriggerExit(Collider other) { }
}