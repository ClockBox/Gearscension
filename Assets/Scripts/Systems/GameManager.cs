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
    public Transform LevelSpawn;
    private static int currentFloor;

    private AudioDictonary audioManager;
    public AudioDictonary AudioManager
    {
        get { return audioManager; }
        set { audioManager = value; }
    }

    private static PlayerController player;
    public static PlayerController Player
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

    private static PlayerHud hud;
    public static PlayerHud Hud
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
    private static bool pause = false;
    public bool Pause
    {
        get { return pause; }
        set { pause = value; }
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else transform.GetChild(0).gameObject.SetActive(false);

        if (!Player && SceneManager.GetActiveScene().buildIndex > 4)
            SpawnPlayer();
    }

    private void Start()
    {
        checkpoint = LevelSpawn;
        gameOver = false;
    }
    #endregion

    private void Update()
    {
        if (!Instance)
        {
            Instance = this;
            transform.GetChild(0).gameObject.SetActive(true);
        }

        if (this != Instance || gameOver)
            return;

        if (Input.GetButtonDown("Pause"))
        {
            if (pause) UnloadScene(pauseMenuScene);
            else AddScene(pauseMenuScene);
        }

        // - DevCodes
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            Instance.checkpoint = null;
            Instance.respawnPoint = player.transform.position;
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
        if (this != Instance)
            return;

        pause = !pause;
        if (Time.timeScale == 1f)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else if (Time.timeScale == 0f)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    public void DestroyObject(GameObject referenceObject)
    {
        Destroy(referenceObject);
    }

    #region Player Managment
    private void SpawnPlayer()
    {   
        Player = Instantiate(playerPrefab, LevelSpawn.position, LevelSpawn.rotation).GetComponent<PlayerController>();
    }

    public void RespawnPlayer()
    {
        if (checkpoint)
            player.transform.position = checkpoint.position;
        else
            player.transform.position = respawnPoint;
        PlayerController.RB.velocity = Vector3.zero;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion

    #region SceneManagment
    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

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

    public void AddNextFloor()
    {
        UnloadScene(SceneManager.GetSceneByBuildIndex(currentFloor));
        AddScene(currentFloor + 1);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void AddScene(string name)
    {
        Debug.Log(name);
        SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
    }
    public void AddScene(int index)
    {
        SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
    }

    public void UnloadScene(string name)
    {
        if(SceneManager.GetSceneByName(name).isLoaded)
            SceneManager.UnloadSceneAsync(name);
    }
    public void UnloadScene(int index)
    {
        if (SceneManager.GetSceneAt(index).isLoaded)
            SceneManager.UnloadSceneAsync(index);
    }
    public void UnloadScene(Scene scene)
    {
        if (scene.isLoaded)
            SceneManager.UnloadSceneAsync(scene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this != Instance)
            return;

        if (scene.name == pauseMenuScene)
            TogglePause();

        else if (scene.buildIndex > 4)
        {
            Cursor.visible = false;
            if (!SceneManager.GetSceneByName(hudScene).isLoaded)
                SceneManager.LoadScene(hudScene, LoadSceneMode.Additive);
            currentFloor = scene.buildIndex;
            PlayerPrefs.SetInt("ContinueScene", currentFloor);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(currentFloor));
        }
        else
        {
            pause = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
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
