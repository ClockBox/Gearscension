using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileDriverBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject piston;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Transform extendedPos;

    float z;

    private Vector3 startPos;
    private Vector3 inTransitPos;

    private void Start()
    {
        startPos = piston.transform.position;
        inTransitPos =  extendedPos.position - piston.transform.position;
        StartCoroutine(Extend());
    }

    private void Update()
    {
        
    }

    IEnumerator Extend()
    {
        yield return new WaitForSeconds(2.0f);
        Rigidbody temp = piston.AddComponent<Rigidbody>();
        temp.constraints = RigidbodyConstraints.FreezePositionY;
        temp.useGravity = false;
        temp.AddForce(piston.transform.forward * 100, ForceMode.Impulse);

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Debug.Log("Vel: " + temp.velocity);

        while (temp.velocity.z > 0.01f)
        {
            z = piston.transform.position.z;

            Mathf.Clamp(z, -14.5f, 21.5f);
            piston.transform.position = new Vector3(piston.transform.position.x, piston.transform.position.y, z);
            yield return null;
        }
        yield return null;
    }
    IEnumerator Retract()
    {
        while (inTransitPos.magnitude > 0.01f)
        {
            yield return null;
        }
    }
}
