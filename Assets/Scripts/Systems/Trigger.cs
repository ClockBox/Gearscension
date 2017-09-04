using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Trigger : MonoBehaviour
{
    public GameObject player;
    public bool inArea;

    [SerializeField]
    protected UnityEvent result;              // - Triggers an event when conditions are met.

    [SerializeField,HideInInspector]
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