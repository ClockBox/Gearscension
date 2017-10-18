﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    IEnumerator Start()
    {

        yield return new WaitForSeconds(2f);
        StartCoroutine(Coroutine());
    }

    IEnumerator Coroutine()
    {

        Debug.Log("Main menu called.");
        SceneManager.LoadScene("Main Menu");
        yield return new WaitForSeconds(0f);
    }

}
