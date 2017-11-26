using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalObject : MonoBehaviour
{
    public AudioSource SFXSource;
    public AudioSource AmbientSource;
    public ParticleSystem runningParticle;
    public GameObject[] lights;

    [Space(20)]
    public ParticleSystem activationParticle;
    public AudioClip[] activationSounds;

    [Space(20)]
    public ParticleSystem deactivationParticle;
    public AudioClip[] deactivationSounds;

    #region Activation
    protected bool active = false;
    public bool Active
    {
        get { return active; }
        set
        {
            active = value;
            if (active) Activate();
            else Deactivate();
        }
    }
    public virtual void Activate()
    {
        Debug.Log("Activate", this);
        active = true;
        ToggleLights(true);
        if (SFXSource)
        {
            if (activationSounds.Length > 0)
            {
                for (int i = 0; i < activationSounds.Length; i++)
                {
                    SFXSource.clip = activationSounds[i];
                    SFXSource.Play();
                }
            }
            else SFXSource.Play();
        }
        if (AmbientSource) AmbientSource.Play();
        if (activationParticle) activationParticle.Play();
    }
    public virtual void Deactivate()
    {
        Debug.Log("Deactivate", this);
        active = false;
        ToggleLights(false);
        if (SFXSource)
        {
            if (deactivationSounds.Length > 0)
            {
                for (int i = 0; i < deactivationSounds.Length; i++)
                {
                    SFXSource.clip = deactivationSounds[i];
                    SFXSource.Play();
                }
            }
            else SFXSource.Play();
        }
        if (AmbientSource) AmbientSource.Stop();
        if (activationParticle) activationParticle.Stop();
    }
    public virtual void Invert()
    {
        Active = !active;
        ToggleLights();
    }

    public void ToggleLights()
    {
        for (int i = 0; i < lights.Length; i++)
            lights[i].SetActive(!lights[i].activeSelf);
    }
    public void ToggleLights(bool active)
    {
        for (int i = 0; i < lights.Length; i++)
            lights[i].SetActive(active);
    }
    #endregion
}
