using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene scene,LoadSceneMode mode)
    {
        if (!player)
            player = FindObjectOfType<PlayerController>();
        if (scene.buildIndex > 2)
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }
}
