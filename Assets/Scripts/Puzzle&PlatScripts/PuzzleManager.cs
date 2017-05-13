using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private GameObject[] platforms;
    private bool allowRotate;
    
	void Start ()
    {
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        allowRotate = true;
	}
	
	void Update ()
    {
        if (allowRotate)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                GameObject.Find("PuzzleGroup01").BroadcastMessage("Rotate");
                allowRotate = false;

                StartCoroutine(WaitForRotate());

            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                GameObject.Find("PuzzleGroup02").BroadcastMessage("Rotate");
                allowRotate = false;

                StartCoroutine(WaitForRotate());

            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                GameObject.Find("PuzzleGroup03").BroadcastMessage("Rotate");
                allowRotate = false;

                StartCoroutine(WaitForRotate());
            }

        }
    }

    IEnumerator WaitForRotate()
    {
        yield return new WaitForSeconds(5.0f);
        allowRotate = true;
    }
}
