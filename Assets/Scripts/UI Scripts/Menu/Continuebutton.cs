using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Continuebutton : MonoBehaviour
{
    // Bool
    private bool retry = false;

    // Other
    private Button cont;

    private void Awake()
    {
        cont = GetComponent<Button>();
    }

    private void Update()
    {
        Active();
    }

    void Active()
    {
        if (PlayerPrefs.GetInt("ContinueScene") > 0 && retry == false)
        {
            cont.interactable = true;
        }
        else cont.interactable = false;
    }
}
