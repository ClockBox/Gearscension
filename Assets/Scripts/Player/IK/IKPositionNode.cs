using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKPositionNode : MonoBehaviour
{
    protected Collider col;
    protected bool _active = true;

    public Transform rightHand;
    public Transform leftHand;
    public Transform rightFoot;
    public Transform leftFoot;

    public IKPositionNode[] neighbours;
    
    protected virtual void Start()
    {
        col = GetComponent<Collider>();
    }

    public bool Active
    {
        get { return _active; }
    }
}
