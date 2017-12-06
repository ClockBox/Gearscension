using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalObject : MonoBehaviour
{
    public AudioSource SFXSource;
    public AudioClip activationSound;
    public AudioClip deactivationSound;

    [Space(10)]
    public ParticleSystem runningParticle;
    public ParticleSystem activationParticle;
    public ParticleSystem deactivationParticle;

    [Space(10)]
    public GameObject[] lights;

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

        if (activationParticle) activationParticle.Play();
        if (SFXSource)
        {
            SFXSource.PlayOneShot(activationSound);
            SFXSource.Play();
        }
    }
    public virtual void Deactivate()
    {
        Debug.Log("Deactivate", this);
        active = false;
        ToggleLights(false);

        if (activationParticle) activationParticle.Stop();
        if (SFXSource)
        {
            SFXSource.Stop();
            SFXSource.PlayOneShot(deactivationSound);
        }
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
