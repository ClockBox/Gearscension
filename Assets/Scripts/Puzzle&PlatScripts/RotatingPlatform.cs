using UnityEngine;
using System.Collections;

public class RotatingPlatform : MonoBehaviour
{
    float counter = 0;
    float speed, width, height;
    
	void Start ()
    {
        speed = 1;
        width = 1;
        height = 1; 
	}

    void Update()
    {
        transform.Rotate((Vector3.forward * 10) * Time.deltaTime);
        //counter += Time.deltaTime * speed;

        ///*        Rotating Vertical Platform
        //float x = Mathf.Cos(counter) * width;
        //float y = Mathf.Sin(counter) * height + 20;
        //float z = 0;
        //*/

        ///*        Rotating Vertical/ Diagonal
        //float x = Mathf.Cos(counter) * width;
        //float y = Mathf.Sin(counter) * height + 20;
        //float z = Mathf.Sin(counter) * height;
        //*/

        //// Rotating Horizontal Platform
        //float x = Mathf.Cos(counter) * width;
        //float y = 1;
        //float z = Mathf.Sin(counter) * height;

        //transform.LookAt(new Vector3(x, y, z));

        //transform.position = new Vector3(x, y, z);
    }
}
