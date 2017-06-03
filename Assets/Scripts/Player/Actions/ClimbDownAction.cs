using UnityEngine;

public class ClimbDownAction : PlayerState
{
    float startTime = 0.0f;
    float elapsedtime = 0.0f;
    float waitTime;

    Quaternion startRotation;
    Vector3 startPosition;
    ClimbingEdge edge;
    ClimbingNode node;

    Collider col;

    public ClimbDownAction(ClimbingEdge Edge) : base()
    {
        startTime = Time.deltaTime;
        startPosition = Player.transform.position;
        startRotation = Player.transform.rotation;

        edge = Edge;
        node = edge.neighbours[0] as ClimbingNode;

        Player.transform.parent = edge.transform;

        col = Player.GetComponent<Collider>();
        col.enabled = false;

        anim.SetTrigger("climbDown");
        anim.SetBool("braced", !(node).FreeHang);

        IK.SetInitialIKPositions(node.rightHand, node.leftHand, node.rightFoot, node.leftFoot);
        IK.headWeight = 0;

        if (anim.GetBool("braced"))
            waitTime = 1.3f;
        else waitTime = 2.0f;

        rb.velocity = Vector3.zero;
    }

    public override CharacterState UpdateState()
    {
        
        HandleInput();
        return HandleStateChange();
    }

    protected override void HandleInput()
    {
        Vector3 averagePoint;
        Vector3 Offset;
        if (anim.GetBool("braced"))
        {
            averagePoint = (IK.RightHand.position + IK.RightFoot.position + IK.LeftHand.position + IK.LeftFoot.position) / 4;
            Offset = Player.transform.up * 1.2f + Player.transform.forward * 0.3f;
        }
        else
        {
            averagePoint = (IK.RightHand.position + IK.LeftHand.position) / 2;
            Offset = Vector3.up * 1.9f;
        }
        float delta = elapsedtime / waitTime;

        Vector3 lerp1 = Vector3.Lerp(startPosition, edge.transform.position, delta);
        Vector3 lerp2 = Vector3.Lerp(edge.transform.position, averagePoint - Offset, delta * 1.1f);
        Player.transform.position = Vector3.Lerp(lerp1, lerp2, delta);
        Player.transform.rotation = Quaternion.Lerp(startRotation, node.transform.rotation, delta * 2f);
    }

    protected override CharacterState HandleStateChange()
    {
        elapsedtime += Time.deltaTime;
        if (elapsedtime - startTime >= waitTime)
        {
            col.enabled = true;
            return new ClimbState(edge.neighbours[0] as ClimbingNode);
        }
        else return null;
    }

    public override void UpdateIK() { }
}
