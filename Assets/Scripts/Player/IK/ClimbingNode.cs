using UnityEngine;

public class ClimbingNode : IKPositionNode
{
    public Transform rightHand;
    public Transform leftHand;
    public Transform rightFoot;
    public Transform leftFoot;

    public bool test;
    public bool FreeHang;

    int _rotation;

    protected override void Start ()
    {
        base.Start();
        neighbours = new IKPositionNode[8];

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
            _active = Vector3.Dot(-transform.forward, Vector3.up) < 0.9f;
            col.enabled = _active;
            
            if (Vector3.Dot(transform.up, Vector3.up) < 0)
            {
                _rotation = (_rotation + 4) % 8;
                transform.rotation = Quaternion.AngleAxis(180, transform.forward) * transform.rotation;
            }

            if (transform.rotation.eulerAngles.z > 0.5f && transform.rotation.eulerAngles.z < 359.5f)
            {
                _rotation = Mathf.RoundToInt((360 - transform.localEulerAngles.z) / 45);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);

                //rightHand.rotation = Quaternion.Euler(-transform.localEulerAngles);
                //leftHand.rotation = Quaternion.Euler(-transform.localEulerAngles);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawLine(transform.position, rightHand.transform.position);
        Gizmos.DrawLine(transform.position, leftHand.transform.position);
        Gizmos.DrawLine(transform.position, rightFoot.transform.position);
        Gizmos.DrawLine(transform.position, leftFoot.transform.position);
    }

    public int Rotation
    {
        get { return _rotation; }
    }
}
