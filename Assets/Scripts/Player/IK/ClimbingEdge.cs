using UnityEngine;

public class ClimbingEdge : IKPositionNode
{
    protected override void Start()
    {
        base.Start();
        neighbours = new IKPositionNode[1];
    }

    private void Update()
    {
        _active = Vector3.Dot(transform.up, Vector3.up) > 0.8f;
        col.enabled = _active;
    }
}