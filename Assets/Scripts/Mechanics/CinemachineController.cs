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
    public CinemachineFreeLook freelook;
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
            freelook.m_LookAt = cameraPivot.transform;
            freelook.m_Follow = GameManager.Player.transform;
            freelook.m_Priority = 11;

            freelook.m_Lens.FieldOfView = 50;
            freelook.m_Lens.NearClipPlane = 0.01f;

            freelook.m_YAxis.m_MaxSpeed = 3;
            freelook.m_YAxis.m_AccelTime = 1;
            freelook.m_YAxis.m_DecelTime = 1;

            freelook.m_Orbits[0].m_Height = 3.5f;
            freelook.m_Orbits[1].m_Height = 2.4f;
            freelook.m_Orbits[2].m_Height = 0.7f;

            freelook.m_Orbits[0].m_Radius = 1.75f;
            freelook.m_Orbits[1].m_Radius = 2.5f;
            freelook.m_Orbits[2].m_Radius = 2f;

            if (Input.GetJoystickNames().Length > 0)
                freelook.m_XAxis.m_MaxSpeed = 150f;
            else
                freelook.m_XAxis.m_MaxSpeed = 5000;
            
            topRig = freelook.GetRig(0);
            middleRig = freelook.GetRig(1);
            bottomRig = freelook.GetRig(2);

            topRig.LookAt = GameManager.Player.transform;
            middleRig.LookAt = cameraPivot.transform;
            bottomRig.LookAt = cameraPivot.transform;

            topComposer = topRig.GetCinemachineComponent<CinemachineComposer>();
            middleComposer = middleRig.GetCinemachineComponent<CinemachineComposer>();
            bottomComposer = bottomRig.GetCinemachineComponent<CinemachineComposer>();

            topComposer.m_DeadZoneHeight = 0;
            middleComposer.m_DeadZoneHeight = 0;
            bottomComposer.m_DeadZoneHeight = 0;

            topComposer.m_DeadZoneWidth = 0;
            middleComposer.m_DeadZoneWidth = 0;
            bottomComposer.m_DeadZoneWidth = 0;
            
            topComposer.m_TrackedObjectOffset = new Vector3(0, 2f, 0);
            middleComposer.m_TrackedObjectOffset = new Vector3(0, -0.3f, 0);
            bottomComposer.m_TrackedObjectOffset = new Vector3(0, 0, 0);

            topRigTransposer = topRig.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            middleRigTransposer = middleRig.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            bottomRigTransposer = bottomRig.GetCinemachineComponent<CinemachineOrbitalTransposer>();

            cC = freelook.gameObject.AddComponent<CinemachineCollider>();
            cC.m_MinimumDistanceFromTarget = 0.1f;
            cC.m_Strategy = CinemachineCollider.ResolutionStrategy.PullCameraForward;
        }
    }
}
