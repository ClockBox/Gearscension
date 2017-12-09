using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager Instance;

    private static string mainMenuScene = "Main Menu";
    private static string pauseMenuScene = "Pause Menu";
    private static string levelCompleteScene = "Completed Level";
    private static string hudScene = "Hud";
    private static string elevatorScene = "Elevator";

    public GameObject playerPrefab;
    public Transform LevelSpawn;
    private static int currentFloor = 4;

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

        if (!Player && SceneManager.GetActiveScene().buildIndex > 3)
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
            LoadScene("Floor_5");
        else if (Input.GetKeyDown(KeyCode.F8))
            LoadScene("BossFloor1");
    }

    public void OnApplicationFocus(bool focus)
    {
        if (this != Instance)
            return;

        if (SceneManager.GetActiveScene().buildIndex > 4)
        {
            if (!focus && !pause && !Application.isEditor)
                AddScene(pauseMenuScene);
        }
    }

    public void TogglePause()
    {
        if (this != Instance)
            return;

        pause = !pause;
        Time.timeScale = pause ? 0 : 1;
        ToggleCursor(pause);
    }

    public void TogglePause(bool pause)
    {
        if (this != Instance)
            return;

        GameManager.pause = pause;
        Time.timeScale = pause ? 0 : 1;
        ToggleCursor(pause);
    }

    public void ToggleCursor(bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
        int sceneIndex = PlayerPrefs.GetInt("ContinueScene");
        LoadScene(sceneIndex);
        if (sceneIndex > 5 && !SceneManager.GetSceneByName(elevatorScene).isLoaded)
            AddScene(elevatorScene);
    }

    public void AddNextFloor()
    {
        StartCoroutine(NextFloor());
    }

    private IEnumerator NextFloor()
    {
        yield return SceneManager.UnloadSceneAsync(currentFloor);
        yield return SceneManager.LoadSceneAsync(currentFloor + 1, LoadSceneMode.Additive);
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
        Debug.Log("GameManager:AddScene", this);
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

        else if (scene.name == hudScene || scene.name == elevatorScene)
        {
            ToggleCursor(pause);
        }
        else if (scene.buildIndex > 3)
        {
            ToggleCursor(false);

            if (!SceneManager.GetSceneByName(hudScene).isLoaded)
                SceneManager.LoadScene(hudScene, LoadSceneMode.Additive);

            if(scene.name != elevatorScene)
                currentFloor = scene.buildIndex;
            PlayerPrefs.SetInt("ContinueScene", currentFloor);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(currentFloor));
        }
        else
        {
            pause = false;
            ToggleCursor(true);
            Time.timeScale = 1;
            if(player) Destroy(player.gameObject);
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (scene.name == pauseMenuScene)
            TogglePause();
    }

    public void WinLevel()
    {
        SceneManager.LoadScene(levelCompleteScene, LoadSceneMode.Additive);
        gameOver = true;
    }
    #endregion  
}
