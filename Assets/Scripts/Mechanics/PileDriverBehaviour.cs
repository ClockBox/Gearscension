using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileDriverBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject extendedPos;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float time;
    [SerializeField]
    private Rigidbody pistonRB;

    [SerializeField]
    private Collider colliderToIgnore;

    private Vector3 startPos;
    private Vector3 inTransitPos;

    private void Awake()
    {
        Physics.IgnoreCollision(pistonRB.GetComponent<Collider>(), colliderToIgnore); 
    }

    private void Start()
    {
        startPos = pistonRB.transform.position;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(Extend());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Boss")
        {
            pistonRB.isKinematic = true;
            pistonRB.velocity = Vector3.zero;
        }
    }

    public void StartExtend()
    {
        StartCoroutine(Extend());
    }

    IEnumerator Extend()
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            pistonRB.transform.position = Vector3.MoveTowards(pistonRB.transform.position, extendedPos.transform.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        pistonRB.isKinematic = false;
        pistonRB.velocity = Vector3.zero;

        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Retract());

        yield return null;
    }
    IEnumerator Retract()
    {
        float elapsedTime = 0;
        while (elapsedTime < (time * 5))
        {
            elapsedTime += Time.deltaTime;
            pistonRB.transform.position = Vector3.MoveTowards(pistonRB.transform.position, startPos, (moveSpeed / 5) * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }
}
