using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Unload : MonoBehaviour
{

    public int scene;

    bool unloaded;

    void OnTriggerEnter()
    {
        if (!unloaded)
        {
            unloaded = true;

            GameController.Instance.UnloadScene(scene);
        }
    }

}
