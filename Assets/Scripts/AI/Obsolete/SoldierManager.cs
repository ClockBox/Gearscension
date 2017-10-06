using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierManager : MonoBehaviour {

    public string enemyState;
    public Vector3 searchPosition;
    public GameObject[] enemies;
    public List<GameObject> squad;
    public float squadRange = 50f;
    void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("SoldierUnit");
        for (int i=0; i < enemies.Length; i++)
        {
            if (Vector3.Distance(transform.position, enemies[i].transform.position) < squadRange&&enemies[i].GetComponent<AiAction>().activated==false)
            {
                squad.Add(enemies[i]);
                enemies[i].GetComponent<AiAction>().activated = true;
            }
        }

    }

    void Start()
    {
        enemyState = "Idle";
    }
    GameObject GetClosest(List<GameObject> arrayObjects, Vector3 targetPosition)
    {
        GameObject closest = null;
        foreach (GameObject t in arrayObjects)
        {
            float dist = Vector3.Distance(t.transform.position, targetPosition);
            float minDist = Mathf.Infinity;
            if (dist < minDist)
            {
                closest = t;
                minDist = dist;
            }
        }
        return closest;
    }

    public void Search()
    {
        GetClosest(squad, searchPosition).GetComponent<AiAction>().searchPos = searchPosition;
        GetClosest(squad, searchPosition).GetComponent<AiAction>().currentTask = "Searching";


    }

    public void Engage()
    {
        enemyState = "Engage";
        for (int i = 0; i < squad.Count; i++)
        {
            squad[i].GetComponent<AiSight>().enabled = false;

            if ((i + 3) % 3 == 0)
                squad[i].GetComponent<AiAction>().currentTask = "Melee";
            else if ((i + 3) % 3 == 1)
                squad[i].GetComponent<AiAction>().currentTask = "Ranged";
            else if ((i + 3) % 3 == 2)
                squad[i].GetComponent<AiAction>().currentTask = "Protect";



        }
    }
}
