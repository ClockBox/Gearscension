using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CalculateNodeNeighbors : MonoBehaviour
{
    public bool Calculate = false;
    public float DetectionRadius = 1.5f;

    ClimbingNode currentNode;
    Collider[] NodeTriggers;
    ClimbingNode[] climbNodes;
    ClimbingEdge[] edgeNodes;

    Vector3[] CompareDirection = new Vector3[8];

    void Start()
    {
        currentNode = GetComponent<ClimbingNode>();

        LayerMask mechanics = 8;
        mechanics = ~mechanics;

        NodeTriggers = Physics.OverlapSphere(transform.position, DetectionRadius, mechanics);

        climbNodes = new ClimbingNode[NodeTriggers.Length];
        for (int i = 0; i < NodeTriggers.Length; i++)
        {
            ClimbingNode checkNode = NodeTriggers[i].GetComponent<ClimbingNode>();
            if (checkNode)
                if (checkNode != climbNodes[i])
                    climbNodes[i] = checkNode;
        }

        edgeNodes = new ClimbingEdge[NodeTriggers.Length];
        for (int i = 0; i < NodeTriggers.Length; i++)
        {
            ClimbingEdge checkNode = NodeTriggers[i].GetComponent<ClimbingEdge>();
            if (checkNode)
                if (checkNode != climbNodes[i])
                    edgeNodes[i] = checkNode;
        }
        
        CompareDirection[0] = transform.up;
        CompareDirection[1] = (transform.up + transform.right).normalized;
        CompareDirection[2] = transform.right;
        CompareDirection[3] = (-transform.up + transform.right).normalized;
        CompareDirection[4] = -transform.up;
        CompareDirection[5] = (-transform.up - transform.right).normalized;
        CompareDirection[6] = -transform.right;
        CompareDirection[7] = (transform.up - transform.right).normalized;
    }

    private void OnDrawGizmos()
    {
        if (currentNode)
        {
            Gizmos.color = Color.red;
            foreach (IKPositionNode neighbor in currentNode.neighbours)
                if (neighbor)
                    Gizmos.DrawLine(transform.position, neighbor.transform.position);
        }
    }

    void Update()
    {
        if (Calculate && Application.isEditor)
        {
            Calculate = false;
            ResetNodes();

            //Check for Climbing Nodes
            foreach (ClimbingNode checkNode in climbNodes)
            {
                if (checkNode && (checkNode != currentNode))
                {
                    for (int i = 0; i < CompareDirection.Length; i++)
                    {
                        Vector3 angle = Quaternion.AngleAxis(checkNode.transform.eulerAngles.y - transform.eulerAngles.y, transform.up) * CompareDirection[i];
                        float compareAngle = 22.5f + 45f * Mathf.Clamp(1f - (checkNode.transform.position - transform.position).magnitude, 0f, 1f);
                        if (Vector3.Angle(angle, checkNode.transform.position - transform.position) < compareAngle)
                        {
                            if (currentNode.neighbours[i] != null)
                            {
                                if ((checkNode.transform.position - transform.position).magnitude < (currentNode.neighbours[i].transform.position - transform.position).magnitude)
                                    currentNode.neighbours[i] = checkNode;
                            }
                            else
                                currentNode.neighbours[i] = checkNode;
                        }
                    }
                }
            }

            //Check for Climbing Edges
            foreach (ClimbingEdge checkNode in edgeNodes)
            {
                if (checkNode && (checkNode != currentNode))
                {
                    Vector3 angle = Quaternion.AngleAxis(checkNode.transform.eulerAngles.y - transform.eulerAngles.y, transform.up) * CompareDirection[0];
                    float compareAngle = 22.5f + 45f * Mathf.Clamp(1f - (checkNode.transform.position - transform.position).magnitude, 0f, 1f);
                    if (Vector3.Angle(angle, checkNode.transform.position - transform.position) < compareAngle)
                    {
                        if (currentNode.neighbours[0] != null)
                        {
                            if ((checkNode.transform.position - transform.position).magnitude < (currentNode.neighbours[0].transform.position - transform.position).magnitude)
                            {
                                currentNode.neighbours[0] = checkNode;
                                checkNode.neighbours[0] = currentNode;
                            }
                        }
                        else
                        {
                            currentNode.neighbours[0] = checkNode;
                            checkNode.neighbours[0] = currentNode;
                        }
                    }
                }
            }
        }
    }

    void ResetNodes()
    {
        for (int i = 0; i < currentNode.neighbours.Length; i++)
        {
            if (currentNode.neighbours[i])
                currentNode.neighbours[i] = null;
        }
    }
}