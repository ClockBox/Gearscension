using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour
{
    // Bool
    private bool allowSceneActivation;
    private bool isDone;


    public void LoadByIndex(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    public void LoadAdd(int Scene)
    {
        SceneManager.LoadSceneAsync(7, LoadSceneMode.Additive);
    }

}
