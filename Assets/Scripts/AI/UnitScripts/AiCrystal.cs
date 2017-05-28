using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCrystal : MonoBehaviour {
    public bool exposed = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "sword"&&exposed==true)
        {
            Destroy(this.gameObject);

        }
    } 
}
