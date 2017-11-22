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

    private Vector3 startPos;
    private Vector3 inTransitPos;

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
