using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    private GameManager gM;

    private void Awake()
    {
        gM = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            gM.RespawnPlayer();
            return;
        }

        Destroy(other.gameObject);
    }
}
