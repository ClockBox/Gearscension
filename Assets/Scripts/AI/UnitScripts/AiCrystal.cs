using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCrystal : MonoBehaviour {
    public bool exposed = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile"&&exposed==true)
        {
            Debug.Log("Dead");
            Destroy(this.gameObject);

        }
    } 
}
