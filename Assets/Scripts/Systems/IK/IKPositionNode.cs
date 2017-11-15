using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Renderer))]
public class IKPositionNode : MonoBehaviour
{
    protected Collider col;
    protected Renderer rend;
    protected bool m_active = true;
    public bool insideWall = false;

    public IKPositionNode[] neighbours;
    public float[] distances;

    int m_rotation;

    protected virtual void Start()
    {
        col = GetComponent<Collider>();
        rend = GetComponent<Renderer>();

        distances = new float[neighbours.Length];
        for (int i = 0; i < distances.Length; i++)
            distances[i] = Mathf.Infinity;
    }
    public virtual void Rotate()
    {
        if (Vector3.Dot(transform.up, Vector3.up) < 0)
        {
            m_rotation = (m_rotation + 4) % 8;
            transform.rotation = Quaternion.AngleAxis(180, transform.forward) * transform.rotation;
        }

        if (transform.rotation.eulerAngles.z > 0.5f && transform.rotation.eulerAngles.z < 359.5f)
        {
            m_rotation = Mathf.RoundToInt((360 - transform.localEulerAngles.z) / 45);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }
    }

    public bool Active
    {
        get { return m_active; }
    }
    public int Rotation
    {
        get { return m_rotation; }
        set { m_rotation = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            insideWall = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!Physics.CheckBox(transform.position + col.bounds.center, col.bounds.extents, transform.rotation))
            insideWall = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player"))
            insideWall = true;
    }

    private void OnCollisionExit(Collision other)
    {
        if (!Physics.CheckBox(transform.position + col.bounds.center, col.bounds.extents, transform.rotation))
            insideWall = false;
    }
}
