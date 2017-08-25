using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Idealy ConditionTypes would be moved to a custom inspector using an EnumMaskField: https://docs.unity3d.com/ScriptReference/EditorGUILayout.EnumMaskField.html
// and would not need to be included in the final build.
// Selecting a type would then create a new condition and add it to the trigger conditions list

[RequireComponent(typeof(Collider))]
public class Trigger : MonoBehaviour
{
    [SerializeField]
    public Condition condish;
    public GameObject player;
    public Trigger t;
    public bool inArea;

    [SerializeField]
    UnityEvent result;              // - Triggers an event when conditions are met.

    [SerializeField]
    public List<Condition> conditions = new List<Condition>();

    public virtual void CheckConditions()
    {
        if (conditions.Count > 0)
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                if (conditions[i].ConditionMet == false)
                    return;
            }
            result.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            inArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            inArea = false;
        }
    }
}