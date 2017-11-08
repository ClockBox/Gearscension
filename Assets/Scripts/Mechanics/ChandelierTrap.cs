using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierTrap : MonoBehaviour
{
    public GameObject[] breakablePart;
    private Rigidbody temp;

    private void Start()
    {
        
    }

    public void DropChandelier()
    {
        for (int i = 0; i < breakablePart.Length; i++)
        {
            temp = breakablePart[i].AddComponent<Rigidbody>();
            temp.useGravity = true;
            temp.constraints = RigidbodyConstraints.None;
        }
    }
}
