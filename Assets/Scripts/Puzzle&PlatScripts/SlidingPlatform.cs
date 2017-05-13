using UnityEngine;
using System.Collections;

public class SlidingPlatform : MonoBehaviour
{
    float counter = 0;
    float speed, width, height;

    void Start()
    {
        speed = 1;
        width = .01f;
        height = 15;
    }

    void Update()
    {
        counter += Time.deltaTime * speed;
        /*              Diagonal Sliding
        float x = Mathf.Cos(counter) * width;
        float y = 0;
        float z = Mathf.Cos(counter) * height;
        */

        /*              Zig-Zag Sliding (Across World)
        float x = Mathf.Cos(counter) * width;
        float y = 0;
        float z = Mathf.Tan(counter) * height;
        */

        /*              Oscilating/Back & Forth
        float x = Mathf.Cos(counter) * width;
        float y = Mathf.Cos(counter) * height + 20;
        float z = Mathf.Sin(counter) * height + 2;
        */

        //              Elevator 
        float x = Mathf.Cos(counter) * width;
        float y = Mathf.Sin(counter) * height + 20;
        float z = Mathf.Sin(counter) * height;

        transform.position = new Vector3(x, y, z);
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
