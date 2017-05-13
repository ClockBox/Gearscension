using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    Transform player;
    Transform cameraLook;

    float rotateSpeed = 5.0f;
    float scale = 2.0f;
    float currentScale = 2.0f;

    Vector3 offset = Vector3.zero;
    
    float elapsedtime = 0.0f;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player)
        {
            offset = player.position + player.up * 5.0f + player.forward * -5.0f;
            cameraLook = player.GetChild(0);
        }
    }

	void Update ()
    {
        if (player)
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotateSpeed, transform.up) * offset;
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * rotateSpeed, transform.right) * offset;
            elapsedtime += Time.deltaTime * 5;

            if (Input.GetButtonDown("Aim") || Input.GetButtonUp("Aim") || EquipmentState.leftTriggerState == 0 ||EquipmentState.leftTriggerState == 1)
                elapsedtime = 0;
            else if (Input.GetButton("Aim") || EquipmentState.leftTriggerState == 2)
            {
                if (scale < 3)
                    scale = Mathf.Lerp(currentScale, 3, elapsedtime);

                transform.position = player.position + (offset - transform.right * 0.5f) / scale;
                transform.LookAt(cameraLook.position - transform.right * 0.5f, player.up);
            }
            else
            {
                currentScale += Input.GetAxis("Zoom");
                currentScale = Mathf.Clamp(currentScale, 1f, 3f);

                if (scale != currentScale)
                    scale = Mathf.Lerp(scale, currentScale, elapsedtime);

                transform.position = player.position + offset / scale;
                transform.LookAt(cameraLook.position, player.up);
            }
        }
    }
}
