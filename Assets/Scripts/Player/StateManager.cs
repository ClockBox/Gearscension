using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private StateTransitions transitions;
    public PlayerState State;

    protected void Start()
    {
        transitions = GetComponent<StateTransitions>();

        //Initialize trigger Delegates
        SetTriggers(State);
        SetCollisions(State);
    }

    //State Functions
    public void StartState(PlayerState state)
    {
        StartCoroutine(state.FixedUpdate());
        StartCoroutine(state.Update());
    }
    public void ChangeState(PlayerState newState)
    {
        StartCoroutine(HandleStateTransition(newState));
    }
    protected IEnumerator HandleStateTransition(PlayerState newState)
    {
        RemoveTriggers(State);
        RemoveCollisions(State);
        SetTriggers(newState);
        SetCollisions(newState);

        newState.InTransition = true;
        yield return StartCoroutine(State.ExitState());
        newState.InTransition = false;

        State = newState;

        State.InTransition = true;
        yield return StartCoroutine(State.EnterState());
        State.InTransition = false;
    }

    public IEnumerator TransitionTo(PlayerState newState)
    {
        if (transitions.Transitions[State.ToString()][newState.ToString()] == transitions.NullTransition)
            Debug.LogError(gameObject.name + ": Transition not set for transition from: " + State.ToString() + "  to: " + newState.ToString());
        yield return transitions.Transitions[State.ToString()][newState.ToString()](State, newState);
    }

    // Trigger Delegates
    delegate void TriggerDelegate(Collider collider);
    private TriggerDelegate triggerEnter;
    private TriggerDelegate triggerStay;
    private TriggerDelegate triggerExit;
    delegate void CollisionDelegate(Collision collision);
    private CollisionDelegate collisionEnter;
    private CollisionDelegate collisionStay;
    private CollisionDelegate collisionExit;
    
    private void RemoveTriggers(PlayerState state)
    {
        triggerEnter -= state.OnTriggerEnter;
        triggerStay -= state.OnTriggerStay;
        triggerExit -= state.OnTriggerExit;
    }
    private void SetTriggers(PlayerState state)
    {
        triggerEnter += state.OnTriggerEnter;
        triggerStay += state.OnTriggerStay;
        triggerExit += state.OnTriggerExit;
    }

    private void RemoveCollisions(PlayerState state)
    {
        collisionEnter -= state.OnCollisionEnter;
        collisionStay -= state.OnCollisionStay;
        collisionExit -= state.OnCollisionExit;
    }
    private void SetCollisions(PlayerState state)
    {
        collisionEnter += state.OnCollisionEnter;
        collisionStay += state.OnCollisionStay;
        collisionExit += state.OnCollisionExit;
    }

    // Unity Trigger Functions Call Current State Tirgger Functions
    private void OnTriggerEnter(Collider other)
    {
        triggerEnter.Invoke(other);
    }
    private void OnTriggerStay(Collider other)
    {
        triggerStay.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        triggerExit.Invoke(other);
    }

    // Unity Collision Functions Call Current State Collision Functions
    private void OnCollisionEnter(Collision collision)
    {
        collisionExit.Invoke(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        collisionStay.Invoke(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        collisionExit.Invoke(collision);
    }
}