using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public static EventSystem ES;

    public float transTime = 0.4f;

    string Hud = "Hud";
    string Player = "CharacterSene";
    string Puzzel = "Platforms";

    string targetScene;
    CanvasGroup canvasGroup;

    // Bool
    private bool gameStart;
    private bool buttonSelected;

    // Other
    public EventSystem eventSystem;
    public GameObject selectedObject;

    // Use this for initialization
    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            canvasGroup = GetComponent<CanvasGroup>();
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        gameStart = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }

        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            TransitionToScene(Player);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            TransitionToScene(Puzzel);
        }

    }

    public void LoadByIndex(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    public void UnloadScene(int Scene)
    {
        StartCoroutine(Unload(Scene));
    }

    void TransitionToScene(string sceneName)
    {
        if(SceneManager.GetActiveScene().name == sceneName)
        {
            return;
        }
        targetScene = sceneName;

        StopCoroutine("TransitionWithAlpha");
        StartCoroutine("TransitionWithAlpha", 1);
    }

    IEnumerator TransitionWithAlpha(float alpha)
    {
        float diff = Mathf.Abs(canvasGroup.alpha - alpha);
        float transtionRate = 0;

        while(diff > 0.02f)
        {
            canvasGroup.alpha = Mathf.SmoothDamp(canvasGroup.alpha, alpha, ref transtionRate, transTime);
            diff = Mathf.Abs(canvasGroup.alpha - alpha);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        canvasGroup.alpha = alpha;
        
        if(alpha == 1)
        {
            StartCoroutine("LoadScene");
        }

    }

    IEnumerator LoadScene()
    {
        SceneManager.LoadScene(targetScene);
        string activeScene = SceneManager.GetActiveScene().name;

        while(activeScene != targetScene)
        {
            Debug.Log("Loading...");
            activeScene = SceneManager.GetActiveScene().name;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        StartCoroutine("TransitionWithAlpha", 0);
    }

    IEnumerator Unload(int scene)
    {
        yield return null;

        SceneManager.UnloadSceneAsync(scene);
    }

    // Sets clickable button to false, If button is Disabpled.
    private void OnDisable()
    {
        buttonSelected = false;
    }

    // closes application, if unity. closes active game.
    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

}

