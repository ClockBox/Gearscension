using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioDictonary : MonoBehaviour {

    public List<AudioClip> AudioClips = new List<AudioClip>();

    [SerializeField]
    private Dictionary<string, AudioClip> AudioClipDictionary = new Dictionary<string, AudioClip>();

    private AudioSource audioPlayer;
    public AudioSource AudioPlayer
    {
        get { return audioPlayer; }
        set { audioPlayer = value; }
    }

    private AudioSource playerSFX;
    public AudioSource PlayerSFX
    {
        get { return playerSFX; }
        set { playerSFX = value; }
    }

    private AudioSource playerVoice;
    public AudioSource PlayerVoice
    {
        get { return playerVoice; }
        set { playerVoice = value; }
    }

    #region MusicIntensityVars

    public int curIntensity;

    private AudioSource int1;
    private AudioSource int2;
    private AudioSource int3;
    private AudioSource int4;
    private AudioSource int5;
    private AudioSource bossroom;

    #endregion

    void Start()
    {

        #region IntensityStuff
        if (gameObject.scene.name != "Elevator" && gameObject.scene.name != "Main Menu" && gameObject.scene.name != "BossFloor")
        {
            int1 = transform.GetChild(0).GetComponent<AudioSource>();
            int2 = transform.GetChild(1).GetComponent<AudioSource>();
            int3 = transform.GetChild(2).GetComponent<AudioSource>();
            int4 = transform.GetChild(3).GetComponent<AudioSource>();
            int5 = transform.GetChild(4).GetComponent<AudioSource>();

            int1.Play();
            int2.Play();
            int3.Play();
            int4.Play();
            int5.Play();
        }


        if (gameObject.scene.name == "Floor_1")
        {
            curIntensity = 1;
        }
        else if (gameObject.scene.name == "Floor_2")
        {
            int1.volume = 0.0f;
            int2.volume = 0.25f;
            curIntensity = 2;
        }
        else if (gameObject.scene.name == "Floor_3")
        {
            int1.volume = 0.0f;
            int3.volume = 0.25f;
            curIntensity = 3;
        }
        else if (gameObject.scene.name == "BossFloor")
        {
            bossroom = transform.GetChild(5).GetComponent<AudioSource>();
            bossroom.Play();
            StartCoroutine(ChangeBossLevelVolume(bossroom, 0.0002f));
        }

        #endregion

        GameManager.Instance.AudioManager = this;
        for (int i = 0; i < AudioClips.Count; i++)
        {
            AudioClipDictionary.Add(Rename(AudioClips[i].name), AudioClips[i]);
        }

        if (GameManager.Player)
        {
            playerSFX = GameManager.Player.SFX;
            playerVoice = GameManager.Player.Voice;
        }
    }

    public void playAudio(string name)
    {
        audioPlayer.PlayOneShot(AudioClipDictionary[name]);
    }

    public void playAudio(AudioSource ap, string name)
    {
        ap.PlayOneShot(AudioClipDictionary[name]);
    }
    public void playAudio(AudioSource ap, AudioClip clip)
    {
        Debug.Log(clip);
        ap.PlayOneShot(clip);
    }

    public void playAudioPlayerSFX(string name)
    {
        playAudio(playerSFX, name);
    }
    public void playAudioPlayerSFX(AudioClip clip)
    {
        playAudio(playerSFX, clip);
    }

    public void playAudioPlayerVoice(string name)
    {
        playAudio(playerVoice, name);
    }
    public void playAudioPlayerVoice(AudioClip clip)
    {
        playAudio(playerVoice, clip);
    }

    public void DivitGrunt()
    {
        int rnd = Mathf.RoundToInt(Random.Range(1.0f, 3.0f));
        playAudio(playerSFX, "divitgrunt"+rnd);
    }

    public void AudioChance(AudioSource ap, AudioClip ac, float chance)
    {
        if (chance < Random.Range(0.0f, 100.0f))
            playAudio(ap, ac);
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && gameObject.scene.name != "BossFloor")
        {
            playAudioPlayerSFX("sfxcowbell");
        }
    }

    #region MusicIntensityChanging
    public void IncreaseIntensity()
    {
        if (curIntensity == 1)
        {
            StartCoroutine(ChangeVolume(int1, int2));
            curIntensity++;
        }
        else if (curIntensity == 2)
        {
            StartCoroutine(ChangeVolume(int2, int3));
            curIntensity++;
        }
        else if (curIntensity == 3)
        {
            StartCoroutine(ChangeVolume(int3, int4));
            curIntensity++;
        }
        else if (curIntensity == 4)
        {
            StartCoroutine(ChangeVolume(int4, int5));
            curIntensity++;
        }
        else
            Debug.Log("Already at max Intensity.");

    }

    private IEnumerator ChangeVolume(AudioSource a, AudioSource b)
    {
        float multplier = 0.005f;
        while (a.volume > 0)
        {
            a.volume -= multplier * Time.deltaTime;
            b.volume += multplier * Time.deltaTime;
            yield return new WaitForSeconds(0.5f);
            multplier += Time.deltaTime;
        }
        a.Stop();
    }
    private IEnumerator ChangeBossLevelVolume(AudioSource a, float delay)
    {
        float multplier = 0.005f;
        while (a.volume <= 0.25f)
        {
            a.volume += multplier * Time.deltaTime;
            yield return new WaitForSeconds(delay*100);
            multplier += Time.deltaTime * delay;
        }
        //playAudio(a, "cathedraltuneint02leviverpaelst");

    }


    #endregion

    //function for remaning the audio file name for the dictionary
    private string Rename(string n)
    {
        string name = "";

        string[] temp = n.Split('_');

        for (int i = 0; i < temp.Length; i++)
        {
            name += temp[i].ToLower();
        }

        return name;
    }
}
