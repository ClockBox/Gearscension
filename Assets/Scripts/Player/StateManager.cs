﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public PlayerState State;

    protected void Start()
    {
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
        yield return StartCoroutine(State.ExitState(newState));
        newState.InTransition = false;

        PlayerState prevState = State;
        State = newState;

        State.InTransition = true;
        yield return StartCoroutine(State.EnterState(prevState));
        State.InTransition = false;

        prevState = null;
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