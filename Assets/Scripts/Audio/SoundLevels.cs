using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundLevels : MonoBehaviour {

    public AudioMixer masterMixer;

    public void SetMasterVol(float vol)
    {
        if (vol == -30.0f) vol = -80.0f;
        masterMixer.SetFloat("masterVol", vol);
    }

    public void SetMusicVol(float vol)
    {
        if (vol == -30.0f) vol = -80.0f;
        masterMixer.SetFloat("musicVol", vol);
    }

    public void SetSFXVol(float vol)
    {
        if (vol == -30.0f) vol = -80.0f;
        masterMixer.SetFloat("sfxVol", vol);
    }

    public void SetAmbienceVol(float vol)
    {
        if (vol == -30.0f) vol = -80.0f;
        masterMixer.SetFloat("ambienceVol", vol);
    }

    public void SetVoiceVol(float vol)
    {
        if (vol == -30.0f) vol = -80.0f;
        masterMixer.SetFloat("voiceVol", vol);
    }

}
