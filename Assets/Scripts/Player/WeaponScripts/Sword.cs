using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private LineRenderer Link;
    public Vector3 bladeOffset;

    private Collider blade;
    public Collider Blade
    {
        get { return blade; }
    }

    public override void Start ()
    {
        base.Start();

        Link = GetComponent<LineRenderer>();
        blade = GetComponentInChildren<Collider>();

        if (!blade) Debug.LogWarning(transform.root.gameObject.name + ": " + name + ": cannot find blade Collider");
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
			other.gameObject.SendMessage("TakeDamage", SendMessageOptions.DontRequireReceiver);
	}
}
