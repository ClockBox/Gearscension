using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    Collider _Collider;

    public override void Start ()
    {
        base.Start();
        _Collider = GetComponentInChildren<Collider>();
        if (!_Collider)
            Debug.LogWarning(transform.root.gameObject.name + ": " + name + ": cannot find blade Collider");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            other.gameObject.SendMessage("TakeDamage", SendMessageOptions.DontRequireReceiver);
    }

    public Collider Blade
    {
        get { return _Collider; }
    }
}
