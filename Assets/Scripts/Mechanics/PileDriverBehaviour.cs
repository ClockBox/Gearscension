using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileDriverBehaviour : MonoBehaviour
{
    public bool isActive;
    [SerializeField]
    private Transform origin;
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

    private bool stop;
    private bool rotate;

    private void Awake()
    {
        Physics.IgnoreCollision(pistonRB.GetComponent<Collider>(), colliderToIgnore);
    }

    private void Start()
    {
        startPos = pistonRB.transform.position;
        isActive = false;
    }

    private void Update()
    {
   
        if(rotate)
        {
            pistonRB.transform.Rotate(0, 0, 1f);
        }
    }
    
    public void Initiate()
    {
        if (isActive)
        {
            isActive = false;
            Debug.Log("HI");
            StopAllCoroutines();
            startPos = origin.position;
            StartExtend();
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Boss")
        {
            StartCoroutine(Extend(extendedPos.transform.position));
            rotate = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Boss")
        {
            Debug.Log("Stop");
            stop = true;
            pistonRB.isKinematic = true;
            pistonRB.velocity = Vector3.zero;
        }
    }

    public void StartRotate()
    {
        rotate = true;
        isActive = true;
    }

    public void StartExtend()
    {
        StartCoroutine(Extend(extendedPos.transform.position));
    }

    IEnumerator Extend(Vector3 position)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            if (stop)
                break;
            elapsedTime += Time.deltaTime;
            pistonRB.transform.position = Vector3.MoveTowards(pistonRB.transform.position,position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        pistonRB.velocity = Vector3.zero;
        StartCoroutine(Retract(startPos));
        yield return null;
    }

    IEnumerator Retract(Vector3 position)
    {
        float elapsedTime = 0;
        while (elapsedTime < time*5)
        {
            if (stop)
                break;
            elapsedTime += Time.deltaTime;
            pistonRB.transform.position = Vector3.MoveTowards(pistonRB.transform.position, position, moveSpeed*0.2f * Time.deltaTime);
            yield return null;
        }
        pistonRB.velocity = Vector3.zero;
        isActive = true;
        yield return null;
    }
}
