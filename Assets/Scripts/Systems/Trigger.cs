using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField, HideInInspector]
    public List<Condition> conditions = new List<Condition>();

    [SerializeField, HideInInspector]
    protected UnityEvent result;
    public UnityEvent Result
    {
        get { return result; }
        set { result = value; }
    }
    
    private void Start()
    {
        for (int i = 0; i < conditions.Count; i++)
            conditions[i].InitCondition(this);
    }

    public virtual bool CheckConditions()
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            if (conditions[i].ConditionMet == false)
                return false;
        }
        result.Invoke();
        Debug.Log("Triggered");
        return true;
    }
}