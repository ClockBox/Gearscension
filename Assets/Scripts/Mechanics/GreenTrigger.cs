using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenTrigger : MonoBehaviour
{
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                gameObject.GetComponentInParent<LightPuzzle>().Invoke("GreenTrigger", 0.1f);
            } 
        }
    }
}
