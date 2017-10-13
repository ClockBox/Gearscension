using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDictonary : MonoBehaviour {

    public List<AudioClip> AudioClips = new List<AudioClip>();

    [SerializeField]
    private Dictionary<string, AudioClip> AudioClipDictionary = new Dictionary<string, AudioClip>();

    void Start()
    {
        GameManager.Instance.AudioManager = this;
        for (int i = 0; i < AudioClips.Count; i++)
        {
            Debug.Log(Rename(AudioClips[i].name));
        }
    }


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
