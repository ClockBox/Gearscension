using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineController : MonoBehaviour
{
    [SerializeField]
    public PlayableDirector playableDirector;

    public void Play()
    {
        playableDirector.Play();
    }
}
