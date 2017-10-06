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

    private static PlayerController player;
    public static PlayerController Player
    {
        get { return player; }
        set { player = value; }
    }
    
    private static GameObject hud;
    public static GameObject Hud
    {
        get { return hud; }
        set { hud = value; }
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

        if (!player)
            player = FindObjectOfType<PlayerController>();
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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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
        if (!player)
            player = FindObjectOfType<PlayerController>();
        if (scene.buildIndex > 2)
        { 
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
            //AudioDictionary = FindObjectOfType<AudioDictionary>();
        }
    }
    #endregion  
}
