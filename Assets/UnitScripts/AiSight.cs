using UnityEngine;
using System.Collections;


public class AiSight : MonoBehaviour {

    public float fieldOfViewAngle = 110f;
    private SphereCollider col;
    GameObject player;
    public float elapsedTime;
    public GameObject managerPrefab;
    GameObject manager;
    void Start () {
        col = GetComponent<SphereCollider>();
        player = GameObject.FindWithTag("Player");
        elapsedTime = 3.5f;
	}
	
	void Update () {

        if (elapsedTime <= 0f)
        {
            yell("Engage");
        }

        if(Vector3.Distance(player.transform.position, transform.position)<4f)
        {
            yell("Engage");

        }

    }

    void OnTriggerStay (Collider other)
    {
       
        if (other.gameObject.tag == "Player")
        {
            
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction.normalized, out hit, col.radius))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        if (Vector3.Distance(other.transform.position, transform.position) <= 15f)
                        {
                            yell("Engage");
                        }
                        else 
                        {
                            yell("Search");
                            manager.GetComponent<SoldierManager>().searchPosition = other.transform.position;
                            elapsedTime -= Time.deltaTime;

                        }
                    }
                   
                }
            }
       


        }

    }

    void yell(string task)
    {
        if (manager == null) 
        manager=Instantiate(managerPrefab, transform.position, transform.rotation);
        manager.SendMessage(task);
    }
}
