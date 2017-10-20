using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseController : MonoBehaviour
{
    private GameManager GM;
	// Use this for initialization
	void Start ()
    {
        GM = GameManager.Instance;
	}

    public void Continue()
    {
        //GM.Pause();
    }

    public void Restart()
    {
        GM.Restart();
    }

    public void MainMenu()
    {
        GM.MainMenu();
    }

    public void EXIT()
    {
        GM.Quit();
    }
}
