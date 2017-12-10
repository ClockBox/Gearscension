using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogArea : MonoBehaviour
{
    public ParticleSystem[] fogLayers;

    public bool fall;
    public bool hasBottom;

    private float speed; 

	void Awake ()
    {
        Vector3 bounds = GetComponent<BoxCollider>().bounds.extents;

        Transform bottom = transform.GetChild(0);

        if (!hasBottom) Destroy(bottom.gameObject);
        else bottom.localScale = new Vector3(transform.localScale.x * bounds.x * 2, transform.localScale.z * bounds.z * 2, 1);

        bounds.y = 1;
        for (int i = 0; i < fogLayers.Length; i++)
        {
            var newshape = fogLayers[i].shape;
            newshape.scale = bounds * 2;
            var newMain = fogLayers[i].main;
            newMain.maxParticles = (int)(bounds.x * bounds.z / 32 * 30);
        }
	}

    private void Update()
    {
        speed = fall ? 5 : 1;
        for (int i = 0; i < fogLayers.Length; i++)
        {
            var newMain = fogLayers[i].main;
            newMain.simulationSpeed = speed;
        }
    }
}