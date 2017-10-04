using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GifSimulate : MonoBehaviour
{
    public Image img;
    private int spriteIndex = 0;

    public Sprite[] fire;
    //private SpriteRenderer rend;

    void Start()
    {
        img = gameObject.GetComponent<Image>();
        // fire = Resources.LoadAll<Sprite>("Fire");

    }

    
    void Update()
    {
        StartCoroutine(SpriteChange());
    }


    IEnumerator SpriteChange()
    {
        img.sprite = fire[spriteIndex];
        // spriteIndex += 1;
        // yield return new WaitForSeconds(5);
        if (spriteIndex >= 24)
        {
            spriteIndex = 0;
            yield return new WaitForSeconds(.2f);
        }
        else
        {
            spriteIndex += 1;
            yield return new WaitForSeconds(5);
        }

    }

}
