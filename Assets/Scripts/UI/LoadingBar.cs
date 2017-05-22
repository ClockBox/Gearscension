using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoadingBar : MonoBehaviour
{

    // Float
    public float fakeIncerment = 0f;
    public float fakeTiming = 0f;

    // Bool
    public bool isFakeLoadingBar = false;


    // Other
    AsyncOperation ao;
    public GameObject LoadingScreenBG;
    public Slider progBar;
    public Text LoadingText;

    public void LoadLevel01()
    {
        LoadingScreenBG.SetActive(true);
        progBar.gameObject.SetActive(true);
        LoadingText.gameObject.SetActive(true);
        LoadingText.text = "Loading...";

        if(!isFakeLoadingBar)
        {
            StartCoroutine(LoadLevelWithRealProgress());
        }
        else
        {
            StartCoroutine(LoadLevelWithFakeProgress());
        }

    }

    IEnumerator LoadLevelWithRealProgress()
    {
        yield return new WaitForSeconds(1);

        ao = SceneManager.LoadSceneAsync(1);
        ao.allowSceneActivation = false;

        while(!ao.isDone)
        {
            progBar.value = ao.progress;

            if(ao.progress == 0.9f)
            {
                progBar.value = 1f;
                LoadingText.text = "Press 'Space Bar' to continue.";
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    ao.allowSceneActivation = true;
                }

            }

            Debug.Log(ao.progress);
            yield return null;
        }

    }

    IEnumerator LoadLevelWithFakeProgress()
    {
        yield return new WaitForSeconds(1);

        while(progBar.value != 1f)
        {
            progBar.value += fakeIncerment;
            yield return new WaitForSeconds(fakeTiming);
        }

        while (progBar.value == 1f)
        {
            LoadingText.text = "Press 'Space Bar' to continue.";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(1);
            }
            yield return null;

        }

    }

}
