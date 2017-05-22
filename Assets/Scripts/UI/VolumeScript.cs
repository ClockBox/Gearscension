using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeScript : MonoBehaviour
{

    // Other
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider sfxVolume;

    [SerializeField] AudioSource masterSound;
    [SerializeField] AudioSource musicSound;
    [SerializeField] AudioSource sfxSound;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        masterSound.volume = musicVolume.value + sfxVolume.value;
        musicSound.volume = musicVolume.value;
        sfxSound.volume = sfxVolume.value;   

	}
}
