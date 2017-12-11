using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRing : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SendMessage("TakeDamage", 50f);
        }
    }
}
