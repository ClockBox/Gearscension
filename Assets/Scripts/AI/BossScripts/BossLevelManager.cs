using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelManager : MonoBehaviour {

    [SerializeField]
    private PointToPointPlatform piston1;
    [SerializeField]
    private Collider[] transitionCol;
    [SerializeField]
    private GameObject stair1;
    [SerializeField]
    private GameObject stair2;
    [SerializeField]
    private GameObject stair3;
    [SerializeField]
    private GameObject ceiling;
    [SerializeField]
    private Transform ceilingPos;
    [SerializeField]
    private Transform stair1Pos;
    [SerializeField]
    private Transform stair2Pos;
    [SerializeField]
    private Transform stair3Pos;

    [SerializeField]
    private GameObject objectGroup;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float speed;
    private bool pistonOne=true;


    // Use this for initialization


    public void ActivatePiston()
    {
        if (pistonOne)
        {
            piston1.GetComponentInChildren<PileDriverBehaviour>().StartRotate();
            piston1.MoveTo(1);
            pistonOne = false;
        }

    }

    public void Transition()
    {


        StartCoroutine (MoveDown(objectGroup,target));
        StartCoroutine(MoveDown(stair1, stair1Pos));
        StartCoroutine(MoveDown(stair2, stair2Pos));
        StartCoroutine(MoveDown(stair3, stair3Pos));
        StartCoroutine(MoveDown(ceiling , ceilingPos));


        for (int i = 0; i < transitionCol.Length; i++)
        {
            transitionCol[i].enabled = true;
        }


    }

    IEnumerator MoveDown(GameObject obj, Transform target)
    {
        while (obj.transform.position.y!= target.position.y)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target.position, speed* Time.deltaTime);
            yield return null;
        }
        Debug.Log("Transition complete");

    }

  
}
