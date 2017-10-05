using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{

    // Int
    private int speed = 3;
    private int speed2 = 4;

    // Other
    private Vector3 start2;
    private Vector3 start;

    [SerializeField] GameObject Cloads;
    [SerializeField] GameObject Cload;

    void Start()
    {
        start = Cload.transform.position;
        start2 = Cloads.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Cload.transform.Translate(-Vector3.right * speed * Time.deltaTime);
        Cloads.transform.Translate(-Vector3.right * speed2 * Time.deltaTime);


        if (Cload.transform.position.x < -5.5f)
        {
            Cload.transform.position = start;
        }
        if (Cloads.transform.position.x < -5.5f)
        {
            Cloads.transform.position = start2;
        }

    }
}
