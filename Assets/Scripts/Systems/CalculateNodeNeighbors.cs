using UnityEngine;

[RequireComponent(typeof(IKPositionNode))]
public class CalculateNodeNeighbors : MonoBehaviour
{
    public float DetectionRadius = 2f;

    IKPositionNode currentNode;
    Collider[] NodeTriggers;
    IKPositionNode[] detectedNodes;

    Vector3[] CompareDirection = new Vector3[8];

    void Start()
    {
        currentNode = GetComponent<IKPositionNode>();

        NodeTriggers = Physics.OverlapSphere(transform.position, DetectionRadius);

        detectedNodes = new IKPositionNode[NodeTriggers.Length];
        for (int i = 0; i < NodeTriggers.Length; i++)
        {
            IKPositionNode checkNode = NodeTriggers[i].GetComponent<IKPositionNode>();
            if (checkNode)
                if (checkNode != detectedNodes[i])
                    detectedNodes[i] = checkNode;
        }
        
        CompareDirection[0] = transform.up;
        CompareDirection[1] = (transform.up + transform.right).normalized;
        CompareDirection[2] = transform.right;
        CompareDirection[3] = (-transform.up + transform.right).normalized;
        CompareDirection[4] = -transform.up;
        CompareDirection[5] = (-transform.up - transform.right).normalized;
        CompareDirection[6] = -transform.right;
        CompareDirection[7] = (transform.up - transform.right).normalized;

        currentNode.Rotate();
        ResetNodes();
        //Check for Climbing Nodes
        foreach (IKPositionNode checkNode in detectedNodes)
        {
            if (checkNode && (checkNode != currentNode))
            {
                if (currentNode as ClimbingEdge)
                {
                    currentNode.neighbours[0] = transform.parent.GetComponent<IKPositionNode>();
                }
                else
                {
                    for (int i = 0; i < CompareDirection.Length; i++)
                    {
                        Vector3 relativeDirection = Quaternion.AngleAxis(checkNode.transform.eulerAngles.y - transform.eulerAngles.y, transform.up) * CompareDirection[i];

                        float compareAngle = 22.5f + 45f * (1f - (checkNode.transform.position - transform.position).magnitude);
                        if (Vector3.Angle(relativeDirection, checkNode.transform.position - transform.position) < compareAngle)
                        {
                            float newDistance = (checkNode.transform.position - transform.position).magnitude;
                            if (currentNode.neighbours[i] != null)
                            {
                                if (newDistance < currentNode.distances[i])
                                {
                                    currentNode.neighbours[i] = checkNode;
                                    currentNode.distances[i] = newDistance;
                                }
                            }
                            else
                            {
                                currentNode.neighbours[i] = checkNode;
                                currentNode.distances[i] = newDistance;
                            }
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