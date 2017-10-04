using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField]
    protected bool checkTriggerOnPlay;
    public bool CheckTriggerOnPlay
    {
        get { return checkTriggerOnPlay; }
        set { checkTriggerOnPlay = value; }
    }

    [SerializeField]
    protected bool repeat;
    public bool Repeat
    {
        get { return repeat; }
        set { repeat = value; }
    }

    [SerializeField, HideInInspector]
    protected List<Condition> conditions = new List<Condition>();
    public List<Condition> Conditions
    {
        get { return conditions; }
        set { conditions = value; }
    }

    [SerializeField, HideInInspector]
    protected UnityEvent result;
    public UnityEvent Result
    {
        get { return result; }
        set { result = value; }
    }

    private bool check = false;

    protected virtual void OnValidate()
    {
        for (int i = 0; i < conditions.Count; i++)
            conditions[i].Trigger = this;
    }

    protected virtual void OnEnable()
    {
        if (checkTriggerOnPlay)
            check = true;
    }
    protected virtual void OnDisable()
    {
        StopAllCoroutines();
        check = false;
    }
    protected virtual void Update()
    {
        if(check) CheckConditions();
    }

    public virtual bool CheckConditions()
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            if (conditions[i].checkCondition() == false)
                return false;
        }

        result.Invoke();
        Debug.Log(this + ":Triggered", this);

        if (!repeat)
        {
            check = false;
            StopAllCoroutines();
        }
        return true;
    }
}