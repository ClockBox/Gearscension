using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterArea : MonoBehaviour {

    public List<GameObject> colliderList;
    bool frozen = false;
    private void Start()
    {
        colliderList = new List<GameObject>();
    }
  
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HI");
        if(!colliderList.Contains(other.gameObject)&&other.gameObject.tag!="Projectile")
        colliderList.Add(other.gameObject);
      
    }
    private void OnTriggerExit(Collider other)
    {
        if (colliderList.Contains(other.gameObject))
        {
            colliderList.Remove(other.gameObject);
        }
    } 
    public void freeze()
    {
        if (!frozen)
        {
            Debug.Log("Freeze" + colliderList.Count);
            for (int i = 0; i < colliderList.Count; i++)
            {
                if (colliderList[i].GetComponent<Rigidbody>())
                {
                    colliderList[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    StartCoroutine(Unfreeze(colliderList[i]));


                }
            }
            frozen = true;
        }
    }

    IEnumerator Unfreeze(GameObject a)
    {
        yield return new WaitForSeconds(3);
        Debug.Log("UNFREEZE");
        a.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        frozen = false;
    }



}
