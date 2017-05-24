using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float m_cameraDistance = 5;

    GameObject Player;
    Vector3 Offset;
    Vector3 AimOffset;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Offset = transform.position - Player.transform.position - (Player.transform.up * 1.8f);
    }

    void Update()
    {
        m_cameraDistance += Input.GetAxis("Zoom") * 2;
        m_cameraDistance = Mathf.Clamp(m_cameraDistance, 2, 8);

        Offset -= transform.up * -Input.GetAxis("Mouse Y") + transform.right * Input.GetAxis("Mouse X");

        if (Input.GetButton("Aim"))
        {
            AimOffset = -Player.transform.right * 0.2f;
            Offset = Offset.normalized;
        }
        else
        {
            AimOffset = Vector3.zero;
            Offset = Offset.normalized * m_cameraDistance;
        }

        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + Offset + AimOffset, 1);

        RaycastHit hit;
        if (Physics.SphereCast(Player.transform.position + Player.transform.up, 0.2f, Offset, out hit, m_cameraDistance))
        {
            if (hit.collider.gameObject != Player)
                transform.position = hit.point + hit.normal * 0.2f;
        }

        transform.rotation = Quaternion.LookRotation(-Offset + AimOffset, Vector3.up);
    }
}