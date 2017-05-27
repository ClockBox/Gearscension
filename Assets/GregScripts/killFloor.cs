using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killFloor : MonoBehaviour {
    public float lifeTime = 3f;
    public float damage = 3;
    public float stunDur = 3f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            other.SendMessageUpwards("TakeDamage", damage);
            other.SendMessageUpwards("Stun", stunDur);

        }
    }
}
