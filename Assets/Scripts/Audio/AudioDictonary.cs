using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDictonary : MonoBehaviour {

    public List<AudioClip> AudioClips = new List<AudioClip>();

    [SerializeField]
    private Dictionary<string, AudioClip> AudioClipDictionary = new Dictionary<string, AudioClip>();

    void Start()
    {
        for (int i = 0; i < AudioClips.Count; i++)
        {
            Debug.Log(AudioClips[i].name);
        }
    }

}
