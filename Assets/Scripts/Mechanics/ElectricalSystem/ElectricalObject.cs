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

    public Animator anim;

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
        Debug.Log(this + ":Activate", this);
        active = true;
        ToggleLights(true);

        if (anim) anim.SetBool("Activated", true);
        if (activationParticle) activationParticle.Play();
        if (runningParticle) runningParticle.Play();
        if (SFXSource)
        {
            SFXSource.Play();
            SFXSource.PlayOneShot(activationSound);
        }
    }
    public virtual void Deactivate()
    {
        Debug.Log(this + " :Deactivate", this);
        active = false;
        ToggleLights(false);

        if (anim) anim.SetBool("Activated", false);
        if (deactivationParticle) deactivationParticle.Play();
        if (runningParticle) runningParticle.Stop();
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
