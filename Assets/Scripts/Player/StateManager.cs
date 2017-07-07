using System.Collections;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public PlayerState State;

    //State Functions
    public void StartState(PlayerState state)
    {
        StartCoroutine(state.FixedUpdate());
        StartCoroutine(state.Update());
    }
    public void ChangeState(PlayerState toState)
    {
        StartCoroutine(HandleStateTransition(toState));
    }

    protected IEnumerator HandleStateTransition(PlayerState toState)
    {
        Debug.Log("From:" + State + " \tTo:" + toState);

        SetTriggers(toState);

        toState.InTransition = true;
        yield return StartCoroutine(State.ExitState());
        toState.InTransition = false;

        State = toState;

        State.InTransition = true;
        yield return StartCoroutine(State.EnterState());
        State.InTransition = false;
    }

    //Trigger Delegates
    delegate void TriggerEnterDelegate(Collider other);
    delegate void TriggerStayDelegate(Collider other);
    delegate void TriggerExitDelegate(Collider other);

    TriggerEnterDelegate triggerEnter;
    TriggerStayDelegate triggerStay;
    TriggerExitDelegate triggerExit;

    private void Start()
    {
        //Initialize Delegates
        triggerEnter += State.OnTriggerEnter;
        triggerStay += State.OnTriggerStay;
        triggerExit += State.OnTriggerExit;
    }
    private void SetTriggers(PlayerState newState)
    {
        triggerEnter -= State.OnTriggerEnter;
        triggerStay -= State.OnTriggerStay;
        triggerExit -= State.OnTriggerExit;

        triggerEnter += newState.OnTriggerEnter;
        triggerStay += newState.OnTriggerStay;
        triggerExit += newState.OnTriggerExit;
    }

    //Trigger Functions
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
}