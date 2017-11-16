using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierTrap : MonoBehaviour
{
    [SerializeField]
    private GameObject breakablePart;

    private Rigidbody temp;
    private bool activated = false;
    private float gravity;
    
    public void DropChandelier()
    {
        if (activated)
            return;

        temp = breakablePart.AddComponent<Rigidbody>();
        temp.gameObject.transform.parent = null;
        Destroy(temp.gameObject, 22.0f);
        activated = true;
    }
}
