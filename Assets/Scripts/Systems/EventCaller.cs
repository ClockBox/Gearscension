using UnityEngine;
using UnityEngine.Events;

public class EventCaller : StateMachineBehaviour
{
    public float EventTime;
    public UnityEvent Event;

    bool eventTriggered = false;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= EventTime && eventTriggered == false)
        {
            eventTriggered = true;
            Event.Invoke();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        eventTriggered = false;
    }
}
