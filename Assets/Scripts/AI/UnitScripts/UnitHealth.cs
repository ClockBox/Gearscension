using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour {
    MonoBehaviour[] scripts;
    public static float armor=5;
    float hitpoint;
    public float regenRate=5;
	// Use this for initialization
	void Start () {
        scripts = gameObject.GetComponents<MonoBehaviour>();
        if (armor==0)
        {
            Debug.Log("armor value not set");
        }
        hitpoint = armor;
	}
	
	// Update is called once per frame
	void Update () {
        if (hitpoint <= 0)
        {
            Stun(1f);
            hitpoint = 1;
        }

        regenRate -= Time.deltaTime;
        if (regenRate <= 0 && hitpoint < armor)
        {
            hitpoint += 1;
            regenRate = 5;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(1);
            Debug.Log(hitpoint);
        }
		
	}
    void Stun(float seconds)
    {
        foreach(MonoBehaviour script in scripts)
        {
            if (script == this)
                continue;
            script.enabled = false;
        }
        breakApart();
        Invoke("Reset", seconds);


    }
    void TakeDamage(float damage)
    {
        hitpoint -= damage;
    }
    private void Reset()
    {
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = true;
        }
    }

    void breakApart()
    {
        AIBreakable[] breakables=gameObject.GetComponentsInChildren<AIBreakable>();
        foreach(AIBreakable breakable in breakables)
        {
            breakable.broken = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            TakeDamage(1);
        }
    } 
}
