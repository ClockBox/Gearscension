using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Trigger : MonoBehaviour
{
    private GameObject player;
    public GameObject Player
    {
        get { return player; }
    }
    public bool InArea;

    [SerializeField, HideInInspector]
    public List<Condition> conditions = new List<Condition>();

    [SerializeField, HideInInspector]
    protected UnityEvent result;

    private void Start()
    {
        for (int i = 0; i < conditions.Count; i++)
            conditions[i].InitCondition(this);
    }

    public virtual void CheckConditions()
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            if (conditions[i].ConditionMet == false)
                return;
        }
        result.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
            InArea = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
            InArea = false;
    }
}