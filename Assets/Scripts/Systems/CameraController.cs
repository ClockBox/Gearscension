using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float Y_ANGLE_MIN = 290;
    private const float Y_ANGLE_MAX = 410.0f;
    
    public Transform camPivot;

    private float distance = 3.0f;
    private float userDistance = 3.0f;
    private float zoom = 45;
    private float normal = 75;
    private float currentX;
    private float currentY;
    private float sensitivity = 3.0f;

    private static float m_zoomed = 0;

    private Camera thisCamera;

    private void Start()
    {
        thisCamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (!camPivot)
            camPivot = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);
        else
        {
            //Mouse Input
            currentX += Input.GetAxis("Mouse X") * sensitivity;
            currentY -= Input.GetAxis("Mouse Y") * sensitivity;
            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

            userDistance -= Input.GetAxis("Zoom") * 2;
            userDistance = Mathf.Clamp(userDistance, 2.5f, 6);

            //Zoom
            distance = userDistance;
            thisCamera.fieldOfView = Mathf.Lerp(normal, zoom, m_zoomed);

            //Set Position and Rotation
            Vector3 moveDirection = new Vector3(0, 0, distance);
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            transform.position = camPivot.position + rotation * moveDirection;
            transform.LookAt(camPivot.position);

            //Camera Raycast
            RaycastHit hit;
            if (Physics.SphereCast(camPivot.position, 0.2f, transform.position - camPivot.position, out hit, distance))
            {
                if (hit.transform.root.gameObject != camPivot.parent.gameObject)
                    transform.position = hit.point + hit.normal * 0.2f;
            }

            //Check distance to player, move camera if to close
            if ((transform.position - camPivot.position).magnitude < 1f)
                currentY -= 5f;
        }
    }

    public static float Zoom
    {
        get { return m_zoomed; }
        set { m_zoomed = value; }
    }
}