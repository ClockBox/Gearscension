using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float m_cameraDistance = 5;

    GameObject Player;
    Vector3 Offset;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Offset = transform.position - Player.transform.position - Player.transform.up;
    }

    void FixedUpdate()
    {
        Offset -= transform.up * Input.GetAxis("Mouse Y") + transform.right * Input.GetAxis("Mouse X");
        Offset = Offset.normalized * m_cameraDistance;
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + Offset, 1);
        transform.rotation = Quaternion.LookRotation(Player.transform.position + (Player.transform.up * 1.5f) - transform.position, Vector3.up);

        RaycastHit hit;
        Debug.DrawLine(Player.transform.position + Player.transform.up, Player.transform.position + Player.transform.up + Offset);
        if (Physics.SphereCast(Player.transform.position + Player.transform.up, 0.1f, Offset, out hit, m_cameraDistance))
        {
            if (hit.collider.gameObject != Player)
                transform.position = hit.point + hit.normal * 0.5f;
        }

        if ((transform.position - Player.transform.position).magnitude < 2)
        {
            Offset += transform.up * (2.5f - (transform.position - Player.transform.position).magnitude);
            Offset += transform.right * 0.2f;
        }
    }
}
