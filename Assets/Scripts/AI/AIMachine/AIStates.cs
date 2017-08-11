using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="AIMachine/AIStates")]

public class AIStates : ScriptableObject {

	public AIActions[] actions;
	public AITransitions[] transitions;

	public Color sceneGizmo = Color.grey;


	public void UpdateState(AIStateManager manager) {


	}

	private void DoActions(AIStateManager manager)
	{
		for (int i = 0; i < actions.Length; i++)
		{
			actions[i].Act(manager);
		}
	}

	private void CheckTransitions(AIStateManager manager)
	{

		for (int i = 0; i < transitions.Length; i++)
		{

			bool decisionSucceeded = transitions[i].decision.Decide(manager);

			if (decisionSucceeded)
			{
				manager.TransitionToState(transitions[i].trueState);
			}
			else
			{
				manager.TransitionToState(transitions[i].falseState);
			}
		}

	}


}
