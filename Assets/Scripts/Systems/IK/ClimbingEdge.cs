using UnityEngine;

public class ClimbingEdge : IKPositionNode
{
    protected override void Start()
    {
        neighbours = new IKPositionNode[1];
        base.Start();

        Rotate();
        Rotation = 0;
    }

    private void Update()
    {
        if (!transform.gameObject.isStatic)
        {
            m_active = Vector3.Dot(transform.up, Vector3.up) > 0.8f;
            col.enabled = m_active;

            Rotate();
        }
    }
}