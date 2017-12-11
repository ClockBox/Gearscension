using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{
    [SerializeField]
    private string[] checkTags;
    [SerializeField]
    private GameObject[] breakablePart;
    [SerializeField]
    private GameObject[] DestroyObjects;

    private Rigidbody temp;
    private Collider[] Cols;

    private void Awake()
    {
        Cols = GetComponents<Collider>();
    }

    void OnTriggerEnter(Collider col)
    {
        for (int t = 0; t < checkTags.Length; t++)
        {
            if (col.gameObject.tag == checkTags[t])
            {
                for (int i = 0; i < Cols.Length; i++)
                    Cols[i].enabled = false;

                for (int i = 0; i < breakablePart.Length; i++)
                {
                    Collider[] temp = breakablePart[i].GetComponents<Collider>();
                    if (temp.Length > 0)
                    {
                        for (int z = 0; z < temp.Length; z++)
                            temp[z].enabled = true;
                    }
                    else breakablePart[i].AddComponent<BoxCollider>();

                    Rigidbody tempRB;
                    if ((tempRB = breakablePart[i].GetComponent<Rigidbody>()))
                        tempRB.isKinematic = false;
                    else breakablePart[i].AddComponent<Rigidbody>();

                    breakablePart[i].transform.parent = null;
                    Destroy(breakablePart[i].gameObject, Random.Range(8, 10));
                }

                for (int i = 0; i < DestroyObjects.Length; i++)
                    Destroy(DestroyObjects[i]);

                Destroy(this);

                break;
            }
        }
    }
}
