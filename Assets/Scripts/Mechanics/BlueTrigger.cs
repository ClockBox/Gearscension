using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueTrigger : MonoBehaviour
{
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                gameObject.GetComponentInParent<LightPuzzle>().Invoke("BlueTrigger", 0.1f);
            } 
        }
    }
}
