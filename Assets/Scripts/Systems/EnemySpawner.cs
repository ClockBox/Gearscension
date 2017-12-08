using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : ElectricalObject
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    public float spawnforce;
    public float spawnDelay;
    public float activationDelay;
    
    private bool canSpawn = true;

    public void SpawnEnemy()
    {
        if (canSpawn) StartCoroutine(SpawnAfterDelay());
    }

    private IEnumerator SpawnAfterDelay()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnDelay);

        anim.SetTrigger("Open");
        var temp = Instantiate(enemyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        temp.tag = "Enemy";
        var enemy = new ComponentContainer(temp);
        enemy.Deactivate();
        enemy.rb.isKinematic = false;
        enemy.rb.AddForce(enemy.rb.mass * spawnPoint.transform.forward * spawnforce, ForceMode.Impulse);

        StartCoroutine(TurnOnEnemy(enemy));
    }

    private IEnumerator TurnOnEnemy(ComponentContainer enemy)
    {
        yield return new WaitForSeconds(activationDelay);
        enemy.Activate();
        enemy.rb.isKinematic = true;
        yield return new WaitForSeconds(1);
        canSpawn = true;
    }

    public class ComponentContainer
    {
        public MonoBehaviour[] scripts;
        public Collider[] colliders;

        public NavMeshAgent agent;
        public Rigidbody rb;

        public ComponentContainer(GameObject enemy)
        {
            scripts = enemy.GetComponents<MonoBehaviour>();
            colliders = enemy.GetComponents<Collider>();

            agent = enemy.GetComponent<NavMeshAgent>();
            rb = enemy.GetComponent<Rigidbody>();
        }

        public void Activate()
        {
            foreach (MonoBehaviour script in scripts)
                script.enabled = true;

            foreach (Collider col in colliders)
                col.enabled = true;

            agent.enabled = true;
            rb.isKinematic = false;
        }

        public void Deactivate()
        {
            foreach (MonoBehaviour script in scripts)
                script.enabled = false;

            foreach (Collider col in colliders)
                col.enabled = false;

            agent.enabled = false;
            rb.isKinematic = true;
        }
    }
}
