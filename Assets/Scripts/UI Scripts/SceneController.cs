using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    string nextScene;
    [SerializeField]
    string lastScene;

    bool prog = false;

    void OnTriggerEnter(Collider col)
    {
        if (prog == false)
        {
            prog = true;
            SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
            //Debug.Log("Loading");
            GameController.Instance.UnloadScene(lastScene);
        }
        else if(prog == true)
        {
            prog = false;
            GameController.Instance.UnloadScene(lastScene);
            SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        }

    }
}
