using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void QuitGame()
    {
        if (Input.GetButtonDown("Quit Game"))
            Application.Quit();


    }
}
