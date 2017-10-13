using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Bool
    public static bool gameIsOver;

    // Strings
    private string mainMenuScene = "Main Menu";
    private string pauseMenuScene = "Pause Menu";
    private string levelCompleteScene = "Completed Level";
    private string gameOverScene = "Game Over";
    private string hudScene = "Hud";
    
    // Other
    public SceneFader sceneFader;

    //private static AudioDictionary audio;
    //public static AudioDictionary Audio
    //{
    //    get { return audio; }
    //    set { audio = value; }
    //}

    private static GameObject player;
    public static GameObject Player
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
            player = FindObjectOfType<PlayerController>().gameObject;

    }

    private void Start()
    {
        gameIsOver = false;
    }

    private void Update()
    {
        AudioListener[] temp = FindObjectsOfType<AudioListener>();
        for (int i = 0; i < temp.Length; i++)
        {
            Debug.Log(temp[i].gameObject, temp[i].gameObject);
        }

        if (gameIsOver)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (Time.timeScale == 1f)
        {
            SceneManager.LoadScene(pauseMenuScene, LoadSceneMode.Additive);
            Time.timeScale = 0f;
        }
        else if (Time.timeScale == 0f)
        {
            SceneManager.UnloadSceneAsync(pauseMenuScene);
            Time.timeScale = 1f;
        }
    }

    public void MainMenu()
    {
        Pause();
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Restart()
    {
        if (gameIsOver)
        {
            SceneManager.UnloadSceneAsync(gameOverScene);
            sceneFader.FadeTo(SceneManager.GetActiveScene().name);
        }
        else
            Pause();

        sceneFader.FadeTo(SceneManager.GetActiveScene().name);

    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;
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
            player = FindObjectOfType<PlayerController>().gameObject;
        if (scene.buildIndex > 3)
        { 
            SceneManager.LoadScene(hudScene, LoadSceneMode.Additive);
            //AudioDictionary = FindObjectOfType<AudioDictionary>();
        }
    }

    private void EndGame()
    {
        SceneManager.LoadScene(gameOverScene, LoadSceneMode.Additive);
        gameIsOver = true;
    }

    private void WinLevel()
    {
        SceneManager.LoadScene(levelCompleteScene, LoadSceneMode.Additive);
        gameIsOver = true;

    }

    #endregion  
}
