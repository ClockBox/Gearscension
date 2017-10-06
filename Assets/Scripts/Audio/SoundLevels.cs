using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundLevels : MonoBehaviour {

    public AudioMixer masterMixer;

    public void SetMasterVol(float vol)
    {
        masterMixer.SetFloat("masterVol", vol);
    }

    public void SetMusicVol(float vol)
    {
        masterMixer.SetFloat("musicVol", vol);
    }

    public void SetSFXVol(float vol)
    {
        masterMixer.SetFloat("sfxVol", vol);
    }

    public void SetAmbienceVol(float vol)
    {
        masterMixer.SetFloat("ambienceVol", vol);
    }

    public void SetVoiceVol(float vol)
    {
        masterMixer.SetFloat("voiceVol", vol);
    }

}
