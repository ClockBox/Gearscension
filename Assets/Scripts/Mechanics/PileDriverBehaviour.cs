using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileDriverBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject piston;
    [SerializeField]
    private float moveSpeed;
    
    private Rigidbody pistonRB;
    
    private Vector3 startPos;
    private Vector3 inTransitPos;

    private void Start()
    {
        startPos = piston.transform.position;
        pistonRB = piston.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(Extend());
        }
    }

    IEnumerator Extend()
    {
        pistonRB.AddForce(piston.transform.forward * moveSpeed * Time.deltaTime, ForceMode.Impulse);
        
        yield return new WaitForSeconds(0.4f);
        pistonRB.velocity = Vector3.zero;

        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Retract());

        yield return null;
    }
    IEnumerator Retract()
    {
        piston.transform.position = startPos;
        
        yield return null;
    }
}
