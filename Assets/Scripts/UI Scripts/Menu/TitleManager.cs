using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    // Public
    public AudioClip listen;
    public AudioClip niceThousand;

    private AudioSource audio;

    private void Awake()
    {
       audio = GetComponent<AudioSource>();
    }

    IEnumerator Start()
    {

        audio.clip = niceThousand;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);

        StartCoroutine(Coroutine());
    }

    IEnumerator Coroutine()
    {

        Debug.Log("Main menu called.");
        SceneManager.LoadScene("Main Menu");
        yield return new WaitForSeconds(0f);
    }

}
