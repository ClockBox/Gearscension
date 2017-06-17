using UnityEngine;
using System.Collections;

public class AiAction : MonoBehaviour
{
    public Vector3 searchPos;
    public bool activated;
    public GameObject[] keyPositions;
    public GameObject[] covers;
    public GameObject[] idlePath;
    int idlePathIndex;
    public float rotateSpeed = 2f;
    public float distanceToNextNode = 1.0f;
    GameObject currentTarget;
    Rigidbody rb;
    public string currentTask;
    public GameObject player;
    bool doneSearch = false;
    public float fireRate = 4f;
    public float meleeRange=2f;
    public float meleeCD=2f;
    UnitPathFinding pathAgent;
    GameObject GetClosest(GameObject[] arrayObjects)
    {
        GameObject closest = null;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in arrayObjects)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            float minDist = Mathf.Infinity;
            if (dist < minDist&&dist>=0.4f)
            {
                closest = t;
                minDist = dist;
            }
        }
        return closest;
    }
    // Use this for initialization
    void Start()
    {
        activated = false;
       
        player = GameObject.FindWithTag("Player");
        covers = GameObject.FindGameObjectsWithTag("CoverNode");
        keyPositions = GameObject.FindGameObjectsWithTag("KeyNode");
        rb = GetComponent<Rigidbody>();
        pathAgent = GetComponent<UnitPathFinding>();

        if (idlePath.Length > 0)
            currentTarget = idlePath[idlePathIndex];

        if (!currentTarget)
        {
            Debug.Log("Target not found.");
        }
        else
        {
            pathAgent.travel(currentTarget.transform.position);
        }

        currentTask = "Idle";
       
    }

    // Update is called once per frame
    void Update()
    {

        if (currentTask == "Idle")

        {

            if ((Vector3.Distance(currentTarget.transform.position, transform.position)) <= distanceToNextNode)
            {

                idlePathIndex++;


                if (idlePathIndex >= idlePath.Length)
                    idlePathIndex = 0;

                currentTarget = idlePath[idlePathIndex];

                pathAgent.travel(currentTarget.transform.position);
            }
        }

        else if (currentTask == "Searching")
        {
             

            if ((Vector3.Distance(searchPos, transform.position)) <= distanceToNextNode)
            {

                doneSearch = true;

            }
           

            if (doneSearch)
            {
                currentTarget = GetClosest(covers);
                pathAgent.travel(currentTarget.transform.position);
                if ((Vector3.Distance(currentTarget.transform.position, transform.position)) <= distanceToNextNode)
                {

                    doneSearch = false;

                }

            }
            else if (!doneSearch)
            {
                pathAgent.travel(searchPos);
            }
        }

        else if (currentTask == "Melee")
			
        {
			if (meleeCD <=0f)
			{
				meleeCD += Time.deltaTime;
			}

			if ((Vector3.Distance(searchPos, transform.position)) <= meleeRange&&meleeCD>=2f)
			{
				Debug.Log("MELEE");
				meleeCD = 0f;
			}

				pathAgent.travel(player.transform.position);


        }
        else if (currentTask == "Ranged")
        {
            
            if (fireRate < 4)
            {
                fireRate += Time.deltaTime;
            }
            

            Vector3 direction = player.transform.position - transform.position;

           
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction.normalized, out hit))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    if (fireRate >=3.9f)
                    {
                        pathAgent.travel(GetClosest(covers).transform.position);

                        GetComponent<EnemyShoot>().rayShot();
                        fireRate = 0;
                        
                    }
                }
                else
                {
                    pathAgent.travel(player.transform.position);

                }
               

            }
           
        
            if (Vector3.Distance(player.transform.position, transform.position) < 3f)
            {
                currentTask = "Melee";

            }


        }
        else if (currentTask == "Protect")
        {
            pathAgent.travel(GetClosest(keyPositions).transform.position);


        }


        }


   
   
 
}


