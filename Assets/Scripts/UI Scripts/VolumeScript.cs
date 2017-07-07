using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeScript : MonoBehaviour
{

    // Other

    public AudioClip listen;
    public AudioClip nineThousand;
    public AudioClip fire;


    [SerializeField] Slider musicVolume;
    [SerializeField] Slider sfxVolume;

    [SerializeField] AudioSource masterSound;
    [SerializeField] AudioSource musicSound;
    [SerializeField] AudioSource sfxSound;

    AudioClip stright;

    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(gameObject);

        if(FindObjectsOfType<VolumeScript>().Length > 1)
        {
            Destroy(gameObject);
        }

        musicSound.clip = fire;
    }
	
	// Update is called once per frame
	void Update ()
    {
        sfxSound.clip = listen;

        musicSound.volume = musicVolume.value;
        sfxSound.volume = sfxVolume.value;
        masterSound.volume = musicVolume.value + sfxVolume.value;

    }
}
