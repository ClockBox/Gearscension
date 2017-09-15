using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop : MonoBehaviour
{
    AmmoSwap ammo;

    public WrapMode wrapMode;
    // Bool
    private bool isPlaying = false;

    private GameObject _object;
    private Animation anim;
    private Animator ator;

    Animation[] animations;

    void Start()
    {


        anim = GetComponent<Animation>();
        ator = GetComponent<Animator>();

        animations = (Animation[])Animation.FindObjectsOfType(typeof(Animation));

    }

    // Update is called once per frame
    void Update()
    {

        ator.SetBool("_isPlaying", isPlaying);

        foreach (Animation a in animations)
        {
            a.wrapMode = WrapMode.Loop;

        }
    }

    void lighting()
    {
        if (isPlaying == true)
        {
            anim.CrossFade("Lighting");
            isPlaying = false;
        }
        else
        {
            anim.CrossFade("Lighting ammo");
            isPlaying = true;
        }
    }
}

