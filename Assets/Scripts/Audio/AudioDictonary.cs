using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        GameManager.Instance.AudioManager = this;
        for (int i = 0; i < AudioClips.Count; i++)
        {
            AudioClipDictionary.Add(Rename(AudioClips[i].name), AudioClips[i]);
        }

        playerSFX = GameManager.Player.SFX;
        playerVoice = GameManager.Player.Voice;
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
