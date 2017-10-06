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
    private Vector3 startPos;
    public Axis axis;

    #region TrapDoor
    //public void TrapDoor(GameObject thingToRotate)
    //{
    //    StartCoroutine(OpenDoor(thingToRotate));
    //}

    //public IEnumerator OpenDoor(GameObject thingToRotate)
    //{
    //    if (!rotated)
    //    {
    //        float x = 140;
    //        while (thingToRotate.transform.rotation.x < 135)
    //        {
    //            thingToRotate.transform.rotation = Quaternion.Slerp(thingToRotate.transform.rotation, Quaternion.Euler(x, 0, 90), moveSpeed * Time.deltaTime);
    //            yield return null;
    //        }
    //        thingToRotate.transform.rotation = Quaternion.Euler(x, 0, 90);
    //        yield return null;
    //    }
    //    else if (rotated)
    //    {
    //        float x = 0;
    //        while (thingToRotate.transform.rotation.x > 0)
    //        {
    //            thingToRotate.transform.rotation = Quaternion.Slerp(thingToRotate.transform.rotation, Quaternion.Euler(x, 0, 90), moveSpeed * Time.deltaTime);
    //            yield return null;
    //        }

    //        if (rotated)
    //            rotated = false;

    //        thingToRotate.transform.rotation = Quaternion.Euler(x, 0, 90);
    //        yield return null;
    //    }
    //}
    #endregion

    #region RotateOverTime
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
    #endregion

    #region RotateObject
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

    public void SetRotateSpeed(float speed)
    {
        rotateSpeed = speed;
    }

    public void RotateObject(Transform thingToRotate)
    {
        startPos = thingToRotate.localEulerAngles;
        StartCoroutine(RotateObj(thingToRotate));
    }

    private IEnumerator RotateObj(Transform thingToRotate)
    {
        angleRotated = 0;
        while (angleRotated < toAngle)
        {
            switch (axis)
            {
                case Axis.X:
                    thingToRotate.Rotate(rotateSpeed * Time.deltaTime * transform.right);
                    break;
                case Axis.Y:
                    thingToRotate.Rotate(rotateSpeed * Time.deltaTime * transform.up);
                    break;
                case Axis.Z:
                    thingToRotate.Rotate(rotateSpeed * Time.deltaTime * transform.forward);
                    break;
            }
            angleRotated += rotateSpeed * Time.deltaTime;
            yield return null;
        }
        thingToRotate.localEulerAngles =
            (axis == Axis.X ? toAngle * thingToRotate.right : Vector3.zero) +
            (axis == Axis.Y ? toAngle * thingToRotate.up : Vector3.zero) +
            (axis == Axis.Z ? toAngle * thingToRotate.forward : Vector3.zero) +
            startPos;


    }
    #endregion

    public void Debuging()
    {
        Debug.Log("PuzzleDone");
    }
}