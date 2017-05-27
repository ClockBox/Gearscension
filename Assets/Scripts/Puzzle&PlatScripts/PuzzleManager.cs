using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private float rotateSpeed;
    public bool allowRotate;
    private Vector3 right;
    private bool rotated = false;
    
    public void TrapDoor(GameObject thingToRotate)
    {
        StartCoroutine(OpenDoor(thingToRotate));
    }
    
    public IEnumerator OpenDoor(GameObject thingToRotate)
    {
        if (!rotated)
        {
            Quaternion fromAngle = thingToRotate.transform.rotation;

            for (var t = 0f; t < 1; t += Time.deltaTime / 3.0f)
            {
                thingToRotate.transform.rotation = Quaternion.Lerp(fromAngle, new Quaternion(90, thingToRotate.transform.rotation.y, thingToRotate.transform.rotation.z, 1), t);
                yield return null;
            }
            rotated = true;
        }
        else
        {
            Quaternion fromAngle = thingToRotate.transform.rotation;

            for (var t = 0f; t < 1; t += Time.deltaTime / 3.0f)
            {
                thingToRotate.transform.rotation = Quaternion.Lerp(fromAngle, new Quaternion(0, thingToRotate.transform.rotation.y, thingToRotate.transform.rotation.z, 1), t);
                yield return null;
            }
            rotated = !rotated;
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