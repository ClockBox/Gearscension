using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private float rotateSpeed;
    public bool allowRotate;
    private Vector3 right;
    private bool rotated = false;
    private float moveSpeed = 0.1f;
    Quaternion startPos;
    
    public void TrapDoor(GameObject thingToRotate)
    {
        StartCoroutine(OpenDoor(thingToRotate));
    }

    public void RotateRoom(GameObject thingToRotate)
    {
        StartCoroutine(Rotate(thingToRotate));
    }
    
    public IEnumerator OpenDoor(GameObject thingToRotate)
    {
        if (!rotated)
        {
            float x = 140;
            while (thingToRotate.transform.rotation.x < 135)
            {
                thingToRotate.transform.rotation = Quaternion.Slerp(thingToRotate.transform.rotation, Quaternion.Euler(x, 0, 90), moveSpeed * Time.deltaTime);
                yield return null;
            }
            thingToRotate.transform.rotation = Quaternion.Euler(x, 0,90);
            yield return null;
        }
        else if(rotated)
        {
            float x = 0;
            while (thingToRotate.transform.rotation.x > 0)
            {
                thingToRotate.transform.rotation = Quaternion.Slerp(thingToRotate.transform.rotation, Quaternion.Euler(x, 0, 90), moveSpeed * Time.deltaTime);
                yield return null;
            }

            if (rotated)
                rotated = false;

            thingToRotate.transform.rotation = Quaternion.Euler(x, 0, 90);
            yield return null;
        }
    }


    public IEnumerator Rotate(GameObject thingToRotate)
    {
        Debug.Log("Start");
        moveSpeed = 0;
        startPos = thingToRotate.transform.rotation;
        while (thingToRotate.transform.rotation.x > 0)
        {
            thingToRotate.transform.rotation = Quaternion.Slerp(thingToRotate.transform.rotation, Quaternion.Euler(0, 0, 0), moveSpeed * Time.deltaTime);
            moveSpeed += Time.deltaTime / 10;
            Debug.Log("Rotating");
            yield return null;
        }
        Debug.Log("End");

        thingToRotate.transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return null;
    }
}