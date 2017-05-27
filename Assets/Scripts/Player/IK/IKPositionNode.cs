using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Renderer))]
public class IKPositionNode : MonoBehaviour
{
    protected Collider col;
    protected Renderer rend;
    protected bool _active = true;

    public IKPositionNode[] neighbours;
    
    protected virtual void Start()
    {
        col = GetComponent<Collider>();
        rend = GetComponent<Renderer>();
    }

    public bool Active
    {
        get { return _active; }
    }
}
