using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private Collider blade;
    public Collider Blade
    {
        get { return blade; }
    }

    public override void Start ()
    {
        base.Start();
        
        blade = GetComponentInChildren<Collider>();

        if (!blade) Debug.LogWarning(transform.root.gameObject.name + ": " + name + ": cannot find blade Collider");
    }

    private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.SendMessage("TakeDamage", SendMessageOptions.DontRequireReceiver);
            GameManager.Instance.AudioManager.playAudioPlayerSFX("sfxswordhit1");
        }
        else GameManager.Instance.AudioManager.playAudioPlayerSFX("sfxswordhit4");
	}
}
