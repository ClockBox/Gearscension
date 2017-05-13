using UnityEngine;
using System.Collections;

public class Point2PointPlatform : MonoBehaviour
{
    public GameObject[] platformPoints;

    Vector3 newPosition;

    public float smooth = 10.0f;

    void Start()
    {
        platformPoints = GameObject.FindGameObjectsWithTag("PlatformPoints");
    }
    
    void FixedUpdate()
    {
        movePlatform(newPosition);

        for (int i = 0; i < 4; i++)
        {
            if (Vector3.Distance(platformPoints[i].transform.position, transform.position) < 1)
                newPosition = platformPoints[i].transform.position;
            else
                newPosition = platformPoints[0].transform.position;
            
        }
    }
    
    void movePlatform(Vector3 newPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, newPos, smooth * Time.deltaTime);
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            c.transform.parent = transform;
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            c.transform.parent = null;
        }
    }
}