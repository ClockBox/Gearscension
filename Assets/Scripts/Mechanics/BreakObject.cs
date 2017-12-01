using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private Transform explosionPoint;

    [SerializeField]
    private AudioClip explosionSound;
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private string checkTag;
    [SerializeField]
    private GameObject[] breakablePart;

    private Rigidbody temp;
    private MeshCollider meshCol;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == checkTag)
            Break();

        if (!explosionPoint)
            explosionPoint = transform;

        if (explosionPrefab)
            Instantiate(explosionPrefab, explosionPoint.position, explosionPoint.rotation);

        GameManager.Instance.AudioManager.playAudio(audioSource, explosionSound.name);
    }

    private void Break()
    {
        for (int i = 0; i < breakablePart.Length; i++)
        {
            if (breakablePart[i].GetComponent<Rigidbody>() == null)
                breakablePart[i].AddComponent<Rigidbody>();
            if (breakablePart[i].GetComponent<BoxCollider>() == null)
                breakablePart[i].AddComponent<BoxCollider>();
            breakablePart[i].layer = LayerMask.NameToLayer("Debris");
            breakablePart[i].transform.parent = null;
            Destroy(breakablePart[i].gameObject, 5);
        }
        GetComponent<Collider>().enabled = false;
    }
}
