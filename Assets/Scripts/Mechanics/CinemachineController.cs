using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Timeline;

public class CinemachineController : MonoBehaviour
{
    [SerializeField]
    private float radius;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject cameraPivot;
    private CinemachineFreeLook freelook;
    private CinemachineVirtualCamera topRig;
    private CinemachineVirtualCamera middleRig;
    private CinemachineVirtualCamera bottomRig;
    [SerializeField]
    private CinemachineVirtualCamera organVCam;

    private CinemachineOrbitalTransposer topRigTransposer;
    private CinemachineOrbitalTransposer middleRigTransposer;
    private CinemachineOrbitalTransposer bottomRigTransposer;

    private CinemachineComposer topComposer;
    private CinemachineComposer middleComposer;
    private CinemachineComposer bottomComposer;

    [SerializeField]
    private TimelinePlayable timelineBase;
    private bool changed = false;

    [SerializeField]
    private float speed;

    private float m_elapsedTime = 20.0f;
    
	void Start ()
    {
        if (player == null)
            player = GameManager.Player.gameObject;

        if (organVCam == null)
            organVCam = GameObject.Find("CM_OrganCam").GetComponent<CinemachineVirtualCamera>();


        if (freelook == null)
        {
            freelook = new GameObject("Freelook").AddComponent<CinemachineFreeLook>();
            freelook.m_LookAt = player.transform;
            freelook.m_Follow = player.transform;
            freelook.m_Priority = 11;
        }

        if (Input.GetJoystickNames().Length > 0)
            freelook.m_XAxis.m_MaxSpeed = 15000f;
        else
            freelook.m_XAxis.m_MaxSpeed = 3000;

        topRig = freelook.GetRig(0);
        middleRig = freelook.GetRig(1);
        bottomRig = freelook.GetRig(2);

        freelook.m_YAxis.m_MaxSpeed = 4;
        freelook.m_YAxis.m_AccelTime = 1;
        freelook.m_YAxis.m_DecelTime = 1;
        
        topComposer = topRig.GetCinemachineComponent<CinemachineComposer>();
        middleComposer = middleRig.GetCinemachineComponent<CinemachineComposer>();
        bottomComposer = bottomRig.GetCinemachineComponent<CinemachineComposer>();

        ResetFreelookDefaults();
    }

    void Update()
    {
        //m_elapsedTime -= Time.deltaTime;

        //if(m_elapsedTime <= 0 && !changed)
        //{
        //    changed = !changed;
        //    ChangeToOrgan();
        //    m_elapsedTime = 5.0f;
        //}
        //else if(m_elapsedTime <= 0 && changed)
        //{
        //    changed = !changed;
        //    ChangeToFreelook();
        //    m_elapsedTime = 5.0f;
        //}

        //Camera Raycast
        RaycastHit hit;
        if (Physics.SphereCast(cameraPivot.transform.position, radius, freelook.transform.position - cameraPivot.transform.position, out hit, 3.0f, LayerMask.NameToLayer("Ground")))
        {
            Debug.Log(hit.transform.gameObject);
            if (hit.transform.gameObject != player )
            {
                topRigTransposer.m_Radius = Mathf.Lerp(topRigTransposer.m_Radius, 0.5f, Time.deltaTime * speed);
                middleRigTransposer.m_Radius = Mathf.Lerp(middleRigTransposer.m_Radius, 0.8f, Time.deltaTime * speed);
                bottomRigTransposer.m_Radius = Mathf.Lerp(bottomRigTransposer.m_Radius, 0.5f, Time.deltaTime * speed);

                //middleComposer.m_TrackedObjectOffset = new Vector3(0, Mathf.Lerp(bottomComposer.m_TrackedObjectOffset.y, 2f, Time.deltaTime * speed), 0);
            }
        }
        else
        {
            topRigTransposer.m_Radius = Mathf.Lerp(topRigTransposer.m_Radius, 2f, Time.deltaTime * speed);
            middleRigTransposer.m_Radius = Mathf.Lerp(middleRigTransposer.m_Radius, 3.3f, Time.deltaTime * speed);
            bottomRigTransposer.m_Radius = Mathf.Lerp(bottomRigTransposer.m_Radius, 2f, Time.deltaTime * speed);

            //middleComposer.m_TrackedObjectOffset = new Vector3(0, Mathf.Lerp(bottomComposer.m_TrackedObjectOffset.y, 0.5f, Time.deltaTime * speed), 0);
        }
    }

    public void ChangeToOrgan()
    {
        freelook.enabled = false;
        organVCam.enabled = true;
        organVCam.Priority = 12;
    }

    public void ChangeToFreelook()
    {
        freelook.enabled = true;
        organVCam.enabled = false;
        organVCam.Priority = 10;
    }

    public void ResetFreelookDefaults()
    {
        // Setting Freelook
        freelook.m_LookAt = cameraPivot.transform;
        freelook.m_Follow = player.transform;
        freelook.m_Priority = 11;

        //Setting Freelook Rigs
        topRig = freelook.GetRig(0);
        middleRig = freelook.GetRig(1);
        bottomRig = freelook.GetRig(2);

        //Setting LookAt
        topRig.LookAt = player.transform;
        middleRig.LookAt = cameraPivot.transform;
        bottomRig.LookAt = cameraPivot.transform;

        //Setting FOV
        topRig.m_Lens.FieldOfView = 50f;
        middleRig.m_Lens.FieldOfView = 50f;
        bottomRig.m_Lens.FieldOfView = 50f;

        //Setting Aim Tracked Offset
        topComposer.m_TrackedObjectOffset = new Vector3(0 ,2f ,0);
        middleComposer.m_TrackedObjectOffset = new Vector3(0, -0.5f, 0);
        bottomComposer.m_TrackedObjectOffset = new Vector3(0, -1f, 0);

        //Setting Rig Transposer
        topRigTransposer = topRig.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        middleRigTransposer = middleRig.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        bottomRigTransposer = bottomRig.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        
        //Setting Rig
        topRigTransposer.m_Radius = 1.8f;
        topRigTransposer.m_HeightOffset = 3.95f;

        middleRigTransposer.m_Radius = 3.4f;
        middleRigTransposer.m_HeightOffset = 2.4f;

        bottomRigTransposer.m_Radius = 1.8f;
        bottomRigTransposer.m_HeightOffset = 0.4f;
    }
}
