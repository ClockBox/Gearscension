using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Transform checkpoint;
    public Transform Checkpoint
    {
        get { return checkpoint; }
        set { checkpoint = value; }
    }

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
    
    private AudioDictonary audioManager;
    public AudioDictonary AudioManager
    {
        get { return audioManager; }
        set { audioManager = value; }
    }

    private PlayerController player;
    public PlayerController Player
    {
        get { return player; }
        set { player = value; }
    }
    
    private PlayerHud hud;
    public PlayerHud Hud
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
    }

    private void Start()
    {
        gameIsOver = false;
    }

    private void Update()
    {
        if (gameIsOver)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void Pause()
    {
        if (Time.timeScale == 1f)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(pauseMenuScene, LoadSceneMode.Additive);
            Time.timeScale = 0f;
        }
        else if (Time.timeScale == 0f)
        {
            Cursor.lockState = CursorLockMode.Locked;
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

    public void RespawnPlayer()
    {
        if (checkpoint)
        {
            player.transform.position = checkpoint.position;
            PlayerController.rb.velocity = Vector3.zero;
        }
        else
            player.transform.position = Vector3.zero;
    }

    public void DestroyObject(GameObject referenceObject)
    {
        Destroy(referenceObject);
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
        if (scene.buildIndex > 3)
            SceneManager.LoadScene(hudScene, LoadSceneMode.Additive);
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
