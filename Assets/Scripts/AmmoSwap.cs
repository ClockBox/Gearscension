using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoSwap : MonoBehaviour
{
    GameManager gamemanager;

    // Bool
    // public bool isPlaying;


    // Other
    public Image current;
    public Image pos1;
    public Image pos2;
    public Image pos3;

    public Sprite fire;
    public Sprite ice;
    public Sprite mask;
    public Sprite magno;

    public GameObject lightprefab;

    // Use this for initialization
    void Start()
    {
        // change later when get another bullet.
        magno = mask;

        lightprefab.GetComponent<Animator>();
        lightprefab.GetComponent<Animation>();
        Fire();
        
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("1"))
        {
            Fire();
        }
        else if (Input.GetButtonDown("2"))
        {
            Ice();
        }
        else if (Input.GetButtonDown("3"))
        {
            Lighting();
        }
        else if (Input.GetButtonDown("4"))
        {
            Magno();
        }
        else { return; }

    }

    /* void Swap()
    {
        if(Input.GetButtonDown("1"))
        {
            current = fire;
            pos1 = ice;
            pos2 = lighting;
            pos3 = magno;
        }
        else if(Input.GetButtonDown("2"))
        {
            current = ice;
            pos1 = lighting;
            pos2 = magno;
            pos3 = fire;
        }
        else if (Input.GetButtonDown("3"))
        {
            current = lighting;
            pos1 = magno;
            pos2 = fire;
            pos3 = ice;
        }
        else if(Input.GetButtonDown("4"))
        {
            current = magno;
            pos1 = fire;
            pos2 = ice;
            pos3 = lighting;
        }
        else
        {
            current = fire;
            pos1 = ice;
            pos2 = magno;
            pos3 = lighting;
        }

    }*/


    void Fire()
    {
        current.sprite = fire;
        pos1.sprite = ice;
        pos2.sprite = mask;
        lightprefab.transform.position = pos2.transform.position;
        pos3.sprite = magno;

    }

    void Ice()
    {
        current.sprite = ice;
        pos1.sprite = mask;
        lightprefab.transform.position = pos1.transform.position;
        pos2.sprite = magno;
        pos3.sprite = fire;

    }

    void Lighting()
    {
        current.sprite = mask;
        lightprefab.transform.position = current.transform.position;
        pos1.sprite = magno;
        pos2.sprite = fire;
        pos3.sprite = ice;

    }

    void Magno()
    {
        current.sprite = magno;
        pos1.sprite = fire;
        pos2.sprite = ice;
        pos3.sprite = mask;
        lightprefab.transform.position = pos3.transform.position;
    }
}
