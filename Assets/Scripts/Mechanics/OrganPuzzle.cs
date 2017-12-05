using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

enum PuzzleState
{
    Idle,
    Intro,
    MainPuzzle,
    Exit
}

public class OrganPuzzle : TimelineController
{
    private AudioDictonary aD;
    private AudioSource audioSource;
    private AudioClip clip;

    private CinemachineVirtualCamera organCam;

    private PlayerState pState;

    private PuzzleState state;

    private bool count;
    private float counter;

    private void Start()
    {
        aD = FindObjectOfType<AudioDictonary>();
        audioSource = GetComponent<AudioSource>();

        organCam = GameObject.Find("CM_OrganCam").GetComponent<CinemachineVirtualCamera>();
        
        clip = aD.AudioClips[13];
    }

    private void Update()
    {
        switch(state)
        {
            case PuzzleState.Intro:
                if (count)
                    counter += Time.deltaTime;

                WaitForHint();
                break;
            case PuzzleState.MainPuzzle:
                if (count)
                    counter += Time.deltaTime;

                if (clip.length + 1  <= counter)
                {
                    return;
                }
                else
                {
                    count = false;
                    counter = 0;
                }

                if(Input.GetButtonDown("Cowbell"))
                {
                    PlayHint();
                    count = true;
                }
                break;
            case PuzzleState.Exit:
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" && Input.GetButtonDown("Action"))
        {
            organCam.Priority = 12;
            count = true;
            Play();
            state = PuzzleState.Intro;
        }
    }

    public void WaitForHint()
    {
        if (playableDirector != null)
            if (playableDirector.duration + 1.0f <= counter)
            {
                aD.playAudio(audioSource, "organhint");
                count = false;
                counter = 0;
                
                state = PuzzleState.MainPuzzle;
            }
    }

    public void LockPlayerControls()
    {

    }

    public void PlayHint()
    {
        aD.playAudio(audioSource, "organhint");
    }

}
