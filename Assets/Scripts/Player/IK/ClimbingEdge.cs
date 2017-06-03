using UnityEngine;

public class ClimbingEdge : IKPositionNode
{
    protected override void Start()
    {
        neighbours = new IKPositionNode[1];
        base.Start();
    }

    private void Update()
    {
        m_active = Vector3.Dot(transform.up, Vector3.up) > 0.8f;
        col.enabled = m_active;
    }
}