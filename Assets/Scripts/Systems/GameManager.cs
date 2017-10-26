using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager Instance;

    private static string mainMenuScene = "Main Menu";
    private static string pauseMenuScene = "Pause Menu";
    private static string levelCompleteScene = "Completed Level";
    private static string gameOverScene = "Game Over";
    private static string hudScene = "Hud";

    public GameObject playerPrefab;

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

    private Vector3 respawnPoint;

    private Transform checkpoint;
    public Transform Checkpoint
    {
        get { return checkpoint; }
        set { checkpoint = value; }
    }

    private PlayerHud hud;
    public PlayerHud Hud
    {
        get { return hud; }
        set { hud = value; }
    }

    private static bool gameOver;
    public bool GameOver
    {
        get { return gameOver; }
        set { gameOver = value; }
    }
    private static bool pause;
    public bool Pause
    {
        get { return pause; }
        set { pause = value; }
    }
    
    public SceneFader sceneFader;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        gameOver = false;
    }
    #endregion

    private void Update()
    {
        if (gameOver)
            return;

        if (Input.GetButtonDown("Pause"))
        {
            if (pause) UnloadScene(pauseMenuScene);
            else AddScene(pauseMenuScene);
        }
        // - DevCodes
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            checkpoint = null;
            respawnPoint = player.transform.position;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
            RespawnPlayer();

        else if (Input.GetKeyDown(KeyCode.F5))
            LoadScene("Floor_1");
        else if (Input.GetKeyDown(KeyCode.F6))
            LoadScene("Floor_2");
        else if (Input.GetKeyDown(KeyCode.F7))
            LoadScene("Floor_3");
        else if (Input.GetKeyDown(KeyCode.F8))
            LoadScene("Floor_4");
    }

    public void TogglePause()
    {
        pause = !pause;
        if (Time.timeScale == 1f)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        else if (Time.timeScale == 0f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Restart()
    {
        if (gameOver)
        {
            SceneManager.UnloadSceneAsync(gameOverScene);
            sceneFader.FadeTo(SceneManager.GetActiveScene().name);
        }
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void RespawnPlayer()
    {
        if (checkpoint)
            player.transform.position = checkpoint.position;
        else
            player.transform.position = respawnPoint;
        PlayerController.rb.velocity = Vector3.zero;
    }

    public void SpawnPlayer()
    {
        if (!player)
            return;
        player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();
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

    #region SceneManagment
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void Continue()
    {
        LoadScene(PlayerPrefs.GetInt("ContinueScene"));
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void LoadScene(Scene scene)
    {
        LoadScene(scene.buildIndex);
    }

    public void AddScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }
    public void AddScene(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Additive);
    }
    public void AddScene(Scene scene)
    {
        AddScene(scene.buildIndex);
    }

    public void UnloadScene(string name)
    {
        SceneManager.UnloadSceneAsync(name);
    }
    public void UnloadScene(int index)
    {
        SceneManager.UnloadSceneAsync(index);
    }
    public void UnloadScene(Scene scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == pauseMenuScene)
            TogglePause();

        if (scene.buildIndex > 3)
        {
            SceneManager.LoadScene(hudScene, LoadSceneMode.Additive);
            PlayerPrefs.SetInt("ContinueScene", scene.buildIndex);
            Debug.Log(PlayerPrefs.GetInt("ContinueScene"));
        }
        else
        {
            pause = false;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 1;
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == pauseMenuScene)
            TogglePause();
    }

    private void EndGame()
    {
        SceneManager.LoadScene(gameOverScene, LoadSceneMode.Additive);
        gameOver = true;
    }

    private void WinLevel()
    {
        SceneManager.LoadScene(levelCompleteScene, LoadSceneMode.Additive);
        gameOver = true;
    }

    #endregion  
}
