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
    
    public void TrapDoor(GameObject thingToRotate)
    {
        StartCoroutine(OpenDoor(thingToRotate));
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
        Quaternion fromAngle = thingToRotate.transform.rotation;
        Quaternion toAngle = Quaternion.Euler(thingToRotate.transform.eulerAngles + new Vector3(0, 60, 0));

        for (var t = 0f; t < 1; t += Time.deltaTime / 3.0f)
        {
            thingToRotate.transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
    }
}