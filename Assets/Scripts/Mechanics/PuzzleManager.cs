using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    X,
    Y,
    Z
}


public class PuzzleManager : MonoBehaviour
{
    private float rotateSpeed;
    public bool allowRotate;
    private Vector3 right;
    private bool rotated = false;
    private bool breakout = false;
    public float angleRotated;
    private float moveSpeed = 0.1f;
    private Vector3 rotateAngle;
    private float toAngle;
    public Axis axis;
    
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

    public void RotateRoom(GameObject thingToRotate)
    {
        StartCoroutine(Rotate(thingToRotate));
    }

    public IEnumerator Rotate(GameObject thingToRotate)
    {
        moveSpeed = 0;

        while (thingToRotate.transform.rotation.x > 0)
        {
            thingToRotate.transform.rotation = Quaternion.Slerp(thingToRotate.transform.rotation, Quaternion.Euler(0, 0, 0), moveSpeed * Time.deltaTime);
            moveSpeed += Time.deltaTime / 10;
            yield return null;
        }

        thingToRotate.transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return null;
    }

    public void SetAxis(string _axis)
    {
        switch (_axis)
        {
            case "X":
                axis = Axis.X;
                break;
            case "Y":
                axis = Axis.Y;
                break;
            case "Z":
                axis = Axis.Z;
                break;
        }
    }

    public void SetAngle(float angle)
    {
        toAngle = angle;
    }

    public void RotateObject(Transform thingToRotate)
    {
        StartCoroutine(RotateObj(thingToRotate));
    }

    private IEnumerator RotateObj(Transform thingToRotate)
    {
        angleRotated = 0;
        while(angleRotated < toAngle)
        {
            Debug.Log(angleRotated);
            switch (axis)
            {
                case Axis.X:
                    thingToRotate.Rotate(2f * Time.deltaTime, 0, 0);
                    break;
                case Axis.Y:
                    thingToRotate.Rotate(0, 2f * Time.deltaTime, 0);
                    break;
                case Axis.Z:
                    thingToRotate.Rotate(0, 0, 2f * Time.deltaTime);
                    break;
            }
            angleRotated += 2f * Time.deltaTime;
            yield return null;
        }
        thingToRotate.localEulerAngles = new Vector3(axis == Axis.X ? toAngle : 0, axis == Axis.Y ? toAngle : 0, axis == Axis.Z ? toAngle : 0);
        
    }
}