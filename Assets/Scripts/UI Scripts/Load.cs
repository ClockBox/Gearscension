using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour
{

    public int scene;

    bool loaded = false;

    void OnTriggerEnter(Collider col)
    {
        if(!loaded)
        {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            Debug.Log("Loading");
            loaded = true;
        }
    }
	
}
