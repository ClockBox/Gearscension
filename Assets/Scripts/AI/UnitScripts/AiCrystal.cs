using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCrystal : MonoBehaviour {
    public bool exposed = false;

    public void TakeDamage()
    {
        if (exposed==true)
        {
            Debug.Log("Dead");
            Destroy(transform.root.gameObject);

        }
    } 
}
