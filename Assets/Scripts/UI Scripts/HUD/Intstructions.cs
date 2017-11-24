using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intstructions : MonoBehaviour
{
    PlayerHud PH;

    // String
    public string passedString;

    private void OnTriggerEnter(Collider other)
    {
        PH.AddDisplay(passedString);
    }

    private void OnTriggerExit(Collider other)
    {
        PH.RemoveDisplay();
    }
}
