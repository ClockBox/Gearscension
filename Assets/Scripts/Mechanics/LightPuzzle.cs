using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPuzzle : MonoBehaviour
{
    public GameObject lightPrefab;
    public GameObject green, blue, red;
    public GameObject[] boxPositioningLights;
    public Material greyMat, greenMat, blueMat, redMat;

    public Light greenLight, redLight, blueLight;
    private int counter;

    private void Awake()
    {

        for (int x = 0; x <= 5; x++)
        {
            GameObject light = Instantiate(lightPrefab,
                boxPositioningLights[x].transform.position,
                boxPositioningLights[x].transform.rotation) as GameObject;
            counter++;
        }
    }
    private void Start()
    {
        counter = 0;
        green.GetComponent<Renderer>().material = greyMat;
        blue.GetComponent<Renderer>().material = greyMat;
        red.GetComponent<Renderer>().material = greyMat;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddRedLight(boxPositioningLights[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddRedLight(boxPositioningLights[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AddRedLight(boxPositioningLights[1]);
        }
    }

    public void AddRedLight(GameObject lightCube)
    {
        if (lightCube.transform.FindChild("RedLight") == null)
        {
            Light light = Instantiate(redLight,
            lightCube.transform.position,
            lightCube.transform.rotation,
            lightCube.transform) as Light;
            light.GetComponent<Renderer>().material = redMat;
        }
    }

    public void AddGreenLight(GameObject lightCube)
    {
        if (lightCube.transform.FindChild("GreenLight") == null)
        {
            Light light = Instantiate(greenLight,
            lightCube.transform.position,
            lightCube.transform.rotation,
            lightCube.transform) as Light;
            light.GetComponent<Renderer>().material = greenMat;
        }
    }

    public void RemoveLights(GameObject lightCube)
    {
        lightCube.GetComponent<Renderer>().material = greyMat;
        
        redLight.enabled = false;

    }
}
