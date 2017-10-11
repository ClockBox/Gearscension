using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //private static AudioDictionary audio;
    //public static AudioDictionary Audio
    //{
    //    get { return audio; }
    //    set { audio = value; }
    //}

    private GameObject player;
    public GameObject Player
    {
        get { return player; }
        set { player = value; }
    }
    
    private static GameObject hud;
    public GameObject Hud
    {
        get { return hud; }
        set { hud = value; }
    }

    private static Transform spawnLocation;
    public Transform SpawnLocation
    {
        get { return spawnLocation; }
        set { spawnLocation = value; }
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetButton("Quit"))
            Quit();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;
    }

    public static void Quit()
    {
        Application.Quit();
    }

    #region SceneManagment
    private void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    private void LoadScene(Scene scene)
    {
        LoadScene(scene.name);
    }
    
    private void AddScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }
    private void AddScene(Scene scene)
    {
        AddScene(scene.name);
    }

    private void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex > 2)
        { 
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
            //AudioDictionary = FindObjectOfType<AudioDictionary>();
        }
    }
    #endregion
    
    public void RespawnPlayer()
    {
        if (player)
        {
            if (spawnLocation)
                player.transform.position = spawnLocation.position;
            else player.transform.position = Vector3.zero;

            PlayerController.rb.velocity = Vector3.zero;
        }
    }
}
