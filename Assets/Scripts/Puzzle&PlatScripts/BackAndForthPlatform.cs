using UnityEngine;
using System.Collections;

public class BackAndForthPlatform : MonoBehaviour
{
    public float speed = 1;

    private int direction = 1;

    private GameObject[] platformNode;

    void Start()
    {
        platformNode = GameObject.FindGameObjectsWithTag("Target");
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * direction * Time.deltaTime);

        if (Vector3.Distance(platformNode[0].transform.position, transform.position) < 1)
        {
            direction = -1;
        }
        else if (Vector3.Distance(platformNode[1].transform.position, transform.position) < 1)
        {
            direction = 1;
        }

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
