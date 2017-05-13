using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected Transform[] IKHandles = new Transform[2];

    public virtual void Start()
    {
        IKHandles[1] = transform.FindChild("Grip_RightHand");
        IKHandles[0] = transform.FindChild("Grip_LeftHand");
    }

    public Transform Grip(int index)
    {
        return IKHandles[index];
    }
}
