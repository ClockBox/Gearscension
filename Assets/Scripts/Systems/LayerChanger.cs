using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LayerChanger : MonoBehaviour
{
    public LayerMask mask;
    public bool find;

    public int Tolayer;
    public bool replace;

    public Collider[] gameObjects;

	void Update ()
    {
        if (find)
        {
            find = false;
            gameObjects = Physics.OverlapBox(Vector3.zero, Vector3.one * 1000, Quaternion.identity, mask);
        }

        if (replace)
        {
            replace = false;
            for (int i = 0; i < gameObjects.Length; i++)
                gameObjects[i].transform.gameObject.layer = Tolayer;
        }
    }
}
