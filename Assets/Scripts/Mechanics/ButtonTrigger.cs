using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonTrigger : MonoBehaviour
{
    public UnityEvent myEvent;

    SlidingPlatform sP;
    PuzzleManager pM;
    
    public void MovePlatform(GameObject platform)
    {
        sP = platform.GetComponent<SlidingPlatform>();
        sP.move = true;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                myEvent.Invoke();
            }
        }
    }
}