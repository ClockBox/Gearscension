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
    private GameObject cameraPivot;
    private CinemachineFreeLook freelook;
    private CinemachineVirtualCamera topRig;
    private CinemachineVirtualCamera middleRig;
    private CinemachineVirtualCamera bottomRig;

    private CinemachineOrbitalTransposer topRigTransposer;
    private CinemachineOrbitalTransposer middleRigTransposer;
    private CinemachineOrbitalTransposer bottomRigTransposer;

    private CinemachineComposer topComposer;
    private CinemachineComposer middleComposer;
    private CinemachineComposer bottomComposer;

    private CinemachineCollider cC;

    [SerializeField]
    private TimelinePlayable timelineBase;
    private bool changed = false;

    [SerializeField]
    private float speed;

    private float m_elapsedTime = 20.0f;
    
	void Start ()
    {
        if (freelook == null)
        {
            freelook = new GameObject("Freelook").AddComponent<CinemachineFreeLook>();
            freelook.m_LookAt = GameManager.Player.transform;
            freelook.m_Follow = GameManager.Player.transform;
            freelook.m_Priority = 11;
        }

        freelook.m_YAxis.m_MaxSpeed = 4;
        freelook.m_YAxis.m_AccelTime = 1;
        freelook.m_YAxis.m_DecelTime = 1;

        if (Input.GetJoystickNames().Length > 0)
            freelook.m_XAxis.m_MaxSpeed = 15000f;
        else
            freelook.m_XAxis.m_MaxSpeed = 500;
       
        freelook.m_LookAt = cameraPivot.transform;
        freelook.m_Follow = GameManager.Player.transform;
        freelook.m_Priority = 11;
        
        topRig = freelook.GetRig(0);
        middleRig = freelook.GetRig(1);
        bottomRig = freelook.GetRig(2);

        topRig.LookAt = GameManager.Player.transform;
        middleRig.LookAt = cameraPivot.transform;
        bottomRig.LookAt = cameraPivot.transform;

        topRig.m_Lens.FieldOfView = 50f;
        middleRig.m_Lens.FieldOfView = 50f;
        bottomRig.m_Lens.FieldOfView = 50f;

        topComposer = topRig.GetCinemachineComponent<CinemachineComposer>();
        middleComposer = middleRig.GetCinemachineComponent<CinemachineComposer>();
        bottomComposer = bottomRig.GetCinemachineComponent<CinemachineComposer>();

        topComposer.m_TrackedObjectOffset = new Vector3(0, 2f, 0);
        middleComposer.m_TrackedObjectOffset = new Vector3(0, -0.5f, 0);
        bottomComposer.m_TrackedObjectOffset = new Vector3(0, -1f, 0);

        topRigTransposer = topRig.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        middleRigTransposer = middleRig.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        bottomRigTransposer = bottomRig.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        
        //topRigTransposer.m_Radius = 1.8f;
        //middleRigTransposer.m_Radius = 3.4f;
        //bottomRigTransposer.m_Radius = 1.8f;

        //topRigTransposer.m_HeightOffset = 3.95f;
        //middleRigTransposer.m_HeightOffset = 2.4f;
        //bottomRigTransposer.m_HeightOffset = 0.4f;
        
        cC = freelook.gameObject.AddComponent<CinemachineCollider>();
        cC.m_MinimumDistanceFromTarget = 0.5f;
    }
}
