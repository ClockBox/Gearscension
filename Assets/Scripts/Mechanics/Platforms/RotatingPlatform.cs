using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : Platform
{
    public Axis axis;
    public float rotateSpeed;
    public bool rotate;

    public bool Rotate
    {
        get { return rotate; }
        set { rotate = value; }
    }

    private void Update()
    {
        if (rotate)
        {
            switch (axis)
            {
                case Axis.X:
                    transform.Rotate(rotateSpeed, 0, 0);
                    break;
                case Axis.Y:
                    transform.Rotate(0, rotateSpeed, 0);
                    break;
                case Axis.Z:
                    transform.Rotate(0, 0, rotateSpeed);
                    break;
            } 
        }
    }
}
