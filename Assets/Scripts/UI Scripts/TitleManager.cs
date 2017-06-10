using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    // Public
    public AudioClip listen;
    public AudioClip niceThousand;


    IEnumerator Start()
    {

        AudioSource audio = GetComponent<AudioSource>();

        audio.clip = listen;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);

        // yield return new WaitForSeconds(1f);

        audio.clip = niceThousand;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);

        StartCoroutine(Coroutine());
    }

    IEnumerator Coroutine()
    {

        yield return new WaitForSeconds(1f);
        Debug.Log("Main menu called.");
        SceneManager.LoadScene("Main Menu");
    }

}
