using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    private bool completed;
    private bool puzzleStarted;
    private bool controlsLocked;
    private PuzzleState state;

    private AudioDictonary aD;
    private AudioSource audioSource;
    private AudioClip clip;

    public UnityEvent myEvent;

    private Camera organCam;
    private CinemachineVirtualCamera organVCam;

    private PlayerController playerController;
    [SerializeField]
    private GameObject playerStandingPos;
    [SerializeField]
    private ParticleSystem[] particles;
    [SerializeField]
    private Canvas canvas;

    private List<int> notes;
    private int noteIndex;
    private static int[] noteOrder = new int[7] {2,3,0,2,3,0,1};

    private bool count;
    private float counter;

    private void Start()
    {
        canvas.enabled = false;
        notes = new List<int>();
        aD = FindObjectOfType<AudioDictonary>();
        audioSource = GetComponent<AudioSource>();
        playerController = FindObjectOfType<PlayerController>();

        organVCam = GameObject.Find("CM_OrganCam").GetComponent<CinemachineVirtualCamera>();
        organCam = GameObject.Find("OrganCam").GetComponent<Camera>();

        organCam.enabled = false;
        
        clip = aD.AudioClips[13];
    }

    private void Update()
    {
        switch(state)
        {
            case PuzzleState.Idle:
                break;
            case PuzzleState.Intro:
                if (count)
                    counter += Time.deltaTime;
                
                WaitForHint();
                break;
            case PuzzleState.MainPuzzle:
                canvas.enabled = true;
                if (count)
                    counter -= Time.deltaTime;

                if (clip.length + 1 < counter)
                {
                    return;
                }
                else
                {
                    count = false;
                }

                Debug.Log("Counter: " + counter);

                if (Input.GetButtonDown("Cowbell"))
                {
                    PlayHint();
                    count = true;
                    counter = clip.length * 2;
                }
                else if (Input.GetButtonDown("Note1"))
                {
                    PlayE();
                }
                else if (Input.GetButtonDown("Note2"))
                {
                    PlayF();
                }
                else if (Input.GetButtonDown("Note3"))
                {
                    PlayB();
                }
                else if (Input.GetButtonDown("Note4"))
                {
                    PlayC();
                }
                else if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Back"))
                {
                    ExitPuzzle();
                }

                if (notes.Count == noteOrder.Length)
                {
                    for (int i = 0; i < notes.Count; i++)
                    {
                        if (notes[i] == noteOrder[i])
                        {
                            continue;
                        }
                        else
                        {
                            notes.Clear();
                            return;
                        }
                    }

                    completed = true;
                    StartCoroutine(PlayOnComplete());
                    myEvent.Invoke();
                    state = PuzzleState.Exit;
                }
                break;
            case PuzzleState.Exit:
                state = PuzzleState.Idle;
                ExitPuzzle();
                canvas.enabled = false;
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" && Input.GetButtonDown("Action") && !completed && !puzzleStarted)
        {
            organCam.enabled = true;
            LockPlayerControls();
            GameManager.Player.transform.position = playerStandingPos.transform.position;
            GameManager.Player.transform.rotation = playerStandingPos.transform.rotation;
            organVCam.Priority = 12;
            count = true;
            Play();
            state = PuzzleState.Intro;
            puzzleStarted = true;
        }
    }

    public void WaitForHint()
    {
        if (playableDirector != null)
            if ((playableDirector.duration / 2) <= counter)
            {
                StartCoroutine(HintParticles());
                aD.playAudio(audioSource, "organhint");
                count = false;
                counter = 0;
                
                state = PuzzleState.MainPuzzle;
            }
    }

    public void LockPlayerControls()
    {
        playerController.PausePlayer();
        controlsLocked = true;
    }

    public void ExitPuzzle()
    {
        playerController.UnPausePlayer();
        organCam.enabled = false;
        controlsLocked = false;
        puzzleStarted = false;
    }

    public void PlayHint()
    {
        StartCoroutine(HintParticles());
        aD.playAudio(audioSource, "organhint");
        notes.Clear();
    }

    //B = 0;
    public void PlayB()
    {
        notes.Add(0);
        if (notes[noteIndex] == noteOrder[noteIndex])
        {
            noteIndex++;
        }
        else
        {
            notes.Clear();
            noteIndex = 0;
        }
        particles[2].Play();
        aD.playAudio(audioSource, "organb36");
    }

    //C = 1
    public void PlayC()
    {
        notes.Add(1);
        if (notes[noteIndex] == noteOrder[noteIndex])
        {
            noteIndex++;
        }
        else
        {
            notes.Clear();
            noteIndex = 0;
        }
        particles[3].Play();
        aD.playAudio(audioSource, "organc7");
    }

    //E = 2
    public void PlayE()
    {
        notes.Add(2);
        if (notes[noteIndex] == noteOrder[noteIndex])
        {
            noteIndex++;
        }
        else
        {
            notes.Clear();
            noteIndex = 0;
        }
        particles[0].Play();
        aD.playAudio(audioSource, "organe14");
    }
    
    //F = 3
    public void PlayF()
    {
        notes.Add(3);
        if (notes[noteIndex] == noteOrder[noteIndex])
        {
            noteIndex++;
        }
        else
        {
            notes.Clear();
            noteIndex = 0;
        }
        particles[1].Play();
        aD.playAudio(audioSource, "organf25");
    }

    public IEnumerator HintParticles()
    {
        int index = 0;
        particles[index].Play();
        yield return new WaitForSeconds(0.25f);
        index = 1;
        particles[index].Play();
        yield return new WaitForSeconds(0.3f);
        index = 2;
        particles[index].Play();
        yield return new WaitForSeconds(0.35f);
        index = 0;
        particles[index].Play();
        yield return new WaitForSeconds(0.25f);
        index = 1;
        particles[index].Play();
        yield return new WaitForSeconds(0.3f);
        index = 2;
        particles[index].Play();
        yield return new WaitForSeconds(0.35f);
        index = 3;
        particles[index].Play();
    }

    public IEnumerator PlayOnComplete()
    {
        yield return new WaitForSeconds(0.5f);
        for(int i =0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
        aD.playAudio(audioSource, "organoncompletion");
    }
    
    public void DebugMessage()
    {
        Debug.Log("Working");
    }
}
