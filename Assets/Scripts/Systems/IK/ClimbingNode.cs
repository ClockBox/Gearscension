using UnityEngine;

public class ClimbingNode : IKPositionNode
{
    static GameObject EdgeNode;

    [Space(10)]
    public Transform rightHand;
    public Transform leftHand;
    public Transform rightFoot;
    public Transform leftFoot;

    [Space(10)]
    public bool FreeHang;
    public bool IsEdge;

    protected override void Start ()
    {
        neighbours = new IKPositionNode[8];
        base.Start();

        if(!EdgeNode) EdgeNode = GameObject.FindGameObjectWithTag("ClimbingEdge");

        if (IsEdge) SpawnEdge();

        if (!rightHand || !leftHand || !rightFoot || !leftFoot)
        {
            Debug.LogError("Climbing Node: " + gameObject.name + " is not set up properly");
            return;
        }
    }

    private void Update()
    {
        if (!transform.gameObject.isStatic)
        {
            FreeHang = Vector3.Dot(-transform.forward, Vector3.up) < -0.5f;
            m_active = Vector3.Dot(-transform.forward, Vector3.up) < 0.9f;
            col.enabled = m_active;

            Rotate();
        }
    }

    private void SpawnEdge()
    {
        ClimbingEdge edge = Instantiate(EdgeNode, transform.position + transform.up * 0.2f, transform.rotation, transform).GetComponent<ClimbingEdge>();
        edge.transform.SetAsFirstSibling();
        edge.neighbours[0] = this;

        neighbours[0] = edge;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawLine(transform.position, rightHand.transform.position);
        Gizmos.DrawLine(transform.position, leftHand.transform.position);
        Gizmos.DrawLine(transform.position, rightFoot.transform.position);
        Gizmos.DrawLine(transform.position, leftFoot.transform.position);

        Gizmos.color = Color.red;
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i])
                Gizmos.DrawLine(transform.position, neighbours[i].transform.position);
        }    
    }
}
