using UnityEngine;

public class ClimbingNode : IKPositionNode
{
    public static ClimbingNode Type;
    private static GameObject EdgeNode;

    private Vector3 playerPosition;
    public Vector3 PlayerPosition
    {
        get {return playerPosition; }
    }

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
        Rotate();
        Rotation = 0;
    }
    private void Update()
    {
        if (!transform.gameObject.isStatic)
            Rotate();
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

    public override void Rotate()
    {
        if (!transform.gameObject.isStatic)
            FreeHang = Vector3.Dot(-transform.forward, Vector3.up) < -0.5f;

        m_active = Vector3.Dot(-transform.forward, Vector3.up) < 0.9f;
        col.enabled = m_active;

        base.Rotate();
        CalculatePlayerPosition();
    }

    private void SpawnEdge()
    {
        ClimbingEdge edge = Instantiate(EdgeNode, transform.position + transform.up * 0.2f, transform.rotation, transform).GetComponent<ClimbingEdge>();
        edge.transform.SetAsFirstSibling();
        edge.neighbours[0] = this;

        neighbours[0] = edge;
    }
    private void CalculatePlayerPosition()
    {
        if (FreeHang)
            playerPosition = transform.position - Vector3.up * 2f;
        else playerPosition = transform.position - transform.forward * 0.4f - transform.up * 1.7f;
    }
}
