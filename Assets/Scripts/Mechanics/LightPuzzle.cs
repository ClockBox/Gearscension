using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightPuzzle : MonoBehaviour
{
    public GameObject green, blue, red;
    public GameObject[] boxPositioningLights;
    public GameObject[] pressurePlates;
    public Material greyMat, greenMat, blueMat, redMat;
    public Light greenLight, blueLight, redLight;
    public UnityEvent myEvent;

    private bool allowChange;
    private int counter;
    private int counterMax;
    private int correctBlockCounter;
    private int[] randomPuzzleSelect, group1, group2, group3;

    private void Start()
    {
        randomPuzzleSelect = new int[boxPositioningLights.Length];

        counterMax = Mathf.RoundToInt(boxPositioningLights.Length / 2);
        Debug.Log(counterMax);
        counter = 0;

        correctBlockCounter = 0;

        group1 = new int[boxPositioningLights.Length];
        group2 = new int[boxPositioningLights.Length];
        group3 = new int[boxPositioningLights.Length];

        InitializePuzzle();

        allowChange = true;
    }

    private void Update()
    {
        if (greenLight.enabled && blueLight.enabled && redLight.enabled)
        {
            for (int i = 0; i < randomPuzzleSelect.Length; i++)
            {
                if (randomPuzzleSelect[i] == 1)
                    TurnOnGreenLight(boxPositioningLights[i]);
                else
                    TurnOnRedLight(boxPositioningLights[i]);
            }
        }
        else if (greenLight.enabled && blueLight.enabled)
        {
            //for (int i = 0; i < randomPuzzleSelect.Length; i++)
            //{
            //    if(boxPositioningLights[i].GetComponentInChildren<Light>().name == "RedLight")
            //        boxPositioningLights[i].GetComponentInChildren<Renderer>().material = redMat;
            //    else if(boxPositioningLights[i].GetComponentInChildren<Light>().name == "GreenLight")
            //        boxPositioningLights[i].GetComponentInChildren<Renderer>().material = greenMat;

            //    boxPositioningLights[i].GetComponentInChildren<Light>().enabled = true;
            //}
            for (int i = 0; i < randomPuzzleSelect.Length; i++)
            {
                if (group1[i] == 1)
                    TurnOnGreenLight(boxPositioningLights[i]);
                else
                    TurnOnRedLight(boxPositioningLights[i]);
            }
        }
        else if (greenLight.enabled && redLight.enabled)
        {
            for (int i = 0; i < randomPuzzleSelect.Length; i++)
            {
                if (group2[i] == 1)
                    TurnOnGreenLight(boxPositioningLights[i]);
                else
                    TurnOnRedLight(boxPositioningLights[i]);
            }
        }
        else if (blueLight.enabled && redLight.enabled)
        {
            for (int i = 0; i < randomPuzzleSelect.Length; i++)
            {
                if (group3[i] == 1)
                    TurnOnGreenLight(boxPositioningLights[i]);
                else
                    TurnOnRedLight(boxPositioningLights[i]);
            }
        }
        else
        {
            for (int i = 0; i < randomPuzzleSelect.Length; i++)
            {
                TurnOffLights(boxPositioningLights[i]);
            }
        }
    }


    public void GreenTrigger()
    {
        if (allowChange)
        {
            Debug.Log("GreenTrigger running...");
            greenLight.enabled = !greenLight.enabled;
            if (greenLight.enabled)
                green.GetComponent<Renderer>().material = greenMat;
            else
                green.GetComponent<Renderer>().material = greyMat;
            allowChange = false;
            StartCoroutine(WaitForLight());
        }
    }

    public void BlueTrigger()
    {
        if (allowChange)
        {
            Debug.Log("BlueTrigger running...");
            blueLight.enabled = !blueLight.enabled;
            if (blueLight.enabled)
                blue.GetComponent<Renderer>().material = blueMat;
            else
                blue.GetComponent<Renderer>().material = greyMat;
            allowChange = false;
            StartCoroutine(WaitForLight());
        }
    }

    public void RedTrigger()
    {
        if (allowChange)
        {
            Debug.Log("RedTrigger running...");
            redLight.enabled = !redLight.enabled;
            if (redLight.enabled)
                red.GetComponent<Renderer>().material = redMat;
            else
                red.GetComponent<Renderer>().material = greyMat;
            allowChange = false;
            StartCoroutine(WaitForLight());
        }
    }

    public void TurnOnRedLight(GameObject lightCube)
    {
        lightCube.transform.GetChild(0).gameObject.SetActive(false);
        lightCube.transform.GetChild(1).gameObject.SetActive(true);
        lightCube.transform.GetChild(0).gameObject.GetComponent<Light>().enabled = false;
        lightCube.transform.GetChild(1).gameObject.GetComponent<Light>().enabled = true;
        lightCube.GetComponent<Renderer>().material = redMat;
    }

    public void TurnOnGreenLight(GameObject lightCube)
    {
        lightCube.transform.GetChild(0).gameObject.SetActive(true);
        lightCube.transform.GetChild(1).gameObject.SetActive(false);
        lightCube.transform.GetChild(0).gameObject.GetComponent<Light>().enabled = true;
        lightCube.transform.GetChild(1).gameObject.GetComponent<Light>().enabled = false;
        lightCube.GetComponent<Renderer>().material = greenMat;
    }

    public void TurnOffLights(GameObject lightCube)
    {
        lightCube.GetComponentInChildren<Light>().enabled = false;
        lightCube.GetComponent<Renderer>().material = greyMat;
    }

    private void InitializePuzzle()
    {
        // Randomized puzzle select
        for (int i = 0; i < randomPuzzleSelect.Length; i++)
        {
            if (counter >= counterMax)
                randomPuzzleSelect[i] = 0;
            else
            {
                randomPuzzleSelect[i] = Random.Range(0, 2);

                if (randomPuzzleSelect[i] == 1)
                    counter++;
            }
        }

        // Checks counter to fill in light locations until full
        if (counter <= counterMax)
        {
            for (int i = randomPuzzleSelect.Length - 1; i > 0; i--)
            {
                if (counter >= counterMax + 1)
                    break;
                if (randomPuzzleSelect[i] == 0)
                {
                    randomPuzzleSelect[i] = 1;
                    counter++;
                }
            }
        }

        // Turns on the lights and disables lights that are un needed
        for (int i = 0; i < randomPuzzleSelect.Length; i++)
        {
            if (randomPuzzleSelect[i] == 1)
                TurnOnGreenLight(boxPositioningLights[i]);
            else
                TurnOnRedLight(boxPositioningLights[i]);
        }

        for (int i = 0; i < randomPuzzleSelect.Length; i++)
        {
            TurnOffLights(boxPositioningLights[i]);
        }


        for (int i = 0; i < randomPuzzleSelect.Length; i++)
        {
            if (i < boxPositioningLights.Length / 3)
            {
                group1[i] = randomPuzzleSelect[i];
                //Debug.Log("Group1");
            }
            else
                group1[i] = 0;
            if (i < (boxPositioningLights.Length / 3) * 2 && i > (boxPositioningLights.Length / 3) - 1)
            {
                group2[i] = randomPuzzleSelect[i];
                //Debug.Log("Group2");
            }
            else
                group2[i] = 0;
            if (i < boxPositioningLights.Length && i >= ((boxPositioningLights.Length / 3) * 2))
            {
                group3[i] = randomPuzzleSelect[i];
                //Debug.Log("Group3");
            }
            else
                group3[i] = 0;
        }

        // Debug for correct puzzle locations
        for (int i = 0; i < randomPuzzleSelect.Length; i++)
            Debug.Log(randomPuzzleSelect[i]);
    }

    public void CheckPuzzlePiece(GameObject goToCheck)
    {
        Debug.Log("CheckPuzzlePiece1");
        for (int i = 0; i < pressurePlates.Length; i++)
        {
            if (goToCheck.name == pressurePlates[i].name && randomPuzzleSelect[i] == 1)
            {
                Debug.Log("CheckPuzzlePiece2");
                correctBlockCounter++;
                Debug.Log(correctBlockCounter);
                CheckPuzzle();
            }
        }
    }

    public void DisengagePuzzlePiece(GameObject goToCheck)
    {
        Debug.Log("CheckPuzzlePiece1");
        for (int i = 0; i < pressurePlates.Length; i++)
        {
            if (goToCheck.name == pressurePlates[i].name && randomPuzzleSelect[i] == 1)
            {
                Debug.Log("Piece removed");
                correctBlockCounter--;
                Debug.Log(correctBlockCounter);
            }
        }
    }

    public void CheckPuzzle()
    {
        if (correctBlockCounter >= counterMax + 1)
        {
            myEvent.Invoke();
            counterMax = 0;
        }
    }

    public IEnumerator WaitForLight()
    {
        yield return new WaitForSeconds(0.5f);
        allowChange = !allowChange;
    }
}