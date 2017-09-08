using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTransitions : MonoBehaviour
{
    protected StateManager manager;

    public string statesFolderPath = "Assets/";
    protected string[] stateTypes;

    public delegate IEnumerator TransitionDelegate(PlayerState fromstate, PlayerState toState);
    public Dictionary<string, Dictionary<string, TransitionDelegate>> Transitions = new Dictionary<string, Dictionary<string, TransitionDelegate>>();

    private void Awake()
    {
        // Access State Names from States Folder
        stateTypes = System.IO.Directory.GetFiles(statesFolderPath, "*State.cs");
        for (int i = 0; i < stateTypes.Length; i++)
        {
            int start = stateTypes[i].LastIndexOf("\\");
            stateTypes[i] = stateTypes[i].Remove(stateTypes[i].Length - 3);
            stateTypes[i] = stateTypes[i].Substring(start + 1);
        }

        // Initilize Transitions Dictionary 
        for (int i = 0; i < stateTypes.Length; i++)
        {
            Dictionary<string, TransitionDelegate> stateDictionary = new Dictionary<string, TransitionDelegate>();
            for (int s = 0; s < stateTypes.Length; s++)
            {
                if (s != i)
                    stateDictionary.Add(stateTypes[s], NullTransition); // Defualt NullTransition
            }
            Transitions.Add(stateTypes[i], stateDictionary);
        }
    }

    protected virtual void Start()
    {
        //Initilaize Transitions

    }

    // NullTransition - all transitions not Initilaized will default to NullTransition;
    public virtual IEnumerator NullTransition(PlayerState fromstate, PlayerState toState)
    {
        yield return null;
    }
}