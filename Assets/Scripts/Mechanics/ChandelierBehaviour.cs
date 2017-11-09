﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject[] breakableParts;

    [SerializeField]
    private GameObject bigCrystal;

    [SerializeField]
    private GameObject smallCrystal;

    [SerializeField]
    private GameObject explosion;

    private Rigidbody temp;
    private bool broken = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Projectile" && other.gameObject.tag != "Chandelier")
        {
            for(int i = 0; i < breakableParts.Length; i++)
            {
                if (breakableParts[i].GetComponent<Rigidbody>() == null)
                {
                    temp = breakableParts[i].AddComponent<Rigidbody>();
                    temp.gameObject.transform.parent = null;
                    Destroy(temp.gameObject, 20.0f);
                }

                else
                {
                    temp = breakableParts[i].GetComponent<Rigidbody>();
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Projectile" || collision.gameObject.tag != "Chandelier")
        {
            if (broken)
                return;
            // Send message to damage enemy

            for(int i = 0; i < 20; i++)
            {
                GameObject spawnedCrystal = Instantiate(smallCrystal,
                    bigCrystal.transform.position,
                    bigCrystal.transform.rotation) as GameObject;

                temp = spawnedCrystal.AddComponent<Rigidbody>();
                spawnedCrystal.transform.parent = null;
                spawnedCrystal.tag = "Chandelier";
                Destroy(spawnedCrystal.gameObject, 20.0f);
            }
            GameObject crystalExplode = Instantiate(explosion,
                bigCrystal.transform.position,
                bigCrystal.transform.rotation) as GameObject;
            Destroy(crystalExplode.gameObject, 5.0f);
            Destroy(bigCrystal.gameObject);
            broken = true;
        }
    }
}
