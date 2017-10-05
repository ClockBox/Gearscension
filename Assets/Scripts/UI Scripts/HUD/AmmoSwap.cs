using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AmmoSwap : MonoBehaviour
{

    // Int
    private int ammo;

    PlayerController PC;

    [Header("Main Hud")]
    [SerializeField] private GameObject healtOne;
    [SerializeField] private GameObject healtTwo;
    [SerializeField] private GameObject healtThree;
    [SerializeField] private GameObject healtFour;
    [SerializeField] private GameObject healtFive;
    [SerializeField] private GameObject healtSix;

    private GameObject[] healthBar;

    private GameObject armorPieceOne;
    private Animator animOne;

    private GameObject armorPieceTwo;
    private Animator animTwo;

    [Header("Ammo Hud")]
    [SerializeField] private Image slot1;
    [SerializeField] private Image slot2;
    [SerializeField] private Image slot3;

    public GameObject[] ammoType = new GameObject[4];

    // Use this for initialization
    void Awake()
    {
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        armorPieceOne = GameObject.Find("/Gear Hud/Armor 1");
        if (armorPieceOne != null)
        {
            animOne = armorPieceOne.GetComponent<Animator>();
        }

        armorPieceTwo = GameObject.Find("/Gear Hud/Armor 2");
        if (armorPieceTwo != null)
        {
            animTwo = armorPieceTwo.GetComponent<Animator>();
        }

        healthBar = new GameObject[]
        {
            healtOne, healtTwo, healtThree, healtFour, healtFive, healtSix
        };

    }


    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
            { BulletUpgrade(); }

        if (ammo != PC.AmmoType)
        {
            ammo = PC.AmmoType;
            setAmmo(ammo);
        }

        //HealthBar();
        ArmorCheck();
        //Bullets();
    }

    private void HealthBar()
    {
        int healthCount = Mathf.RoundToInt(PC.Health / 20);

        if(healthCount >= 5)
        {
            healtSix.SetActive(true);

            healtOne.SetActive(false);
            healtTwo.SetActive(false);
            healtThree.SetActive(false);
            healtFour.SetActive(false);
            healtFive.SetActive(false);
            Debug.Log("Health Set to full");
        }

        else if (healthCount == 4)
        {
            healtFive.SetActive(true);

            healtOne.SetActive(true);
            healtTwo.SetActive(false);
            healtThree.SetActive(false);
            healtFour.SetActive(false);

            healtSix.SetActive(false);
            Debug.Log("Health taken 1 DMG");
        }

        else if (healthCount == 3)
        {
            healtFour.SetActive(true);

            healtOne.SetActive(false);
            healtTwo.SetActive(false);
            healtThree.SetActive(false);

            healtFive.SetActive(false);
            healtSix.SetActive(false);
            Debug.Log("Health taken 2 DMG");
        }

        else if (healthCount == 2)
        {
            healtThree.SetActive(true);

            healtOne.SetActive(false);
            healtTwo.SetActive(false);

            healtFour.SetActive(false);
            healtFive.SetActive(false);
            healtSix.SetActive(false);
            Debug.Log("Health taken 3 DMG");
        }

        else if (healthCount == 1)
        {
            healtTwo.SetActive(true);

            healtOne.SetActive(false);

            healtThree.SetActive(false);
            healtFour.SetActive(false);
            healtFive.SetActive(false);
            healtSix.SetActive(false);
            Debug.Log("Health taken 4 DMG");
        }

        else
        {
            healtOne.SetActive(true);

            healtTwo.SetActive(false);
            healtThree.SetActive(false);
            healtFour.SetActive(false);
            healtFive.SetActive(false);
            healtSix.SetActive(false);
            Debug.Log("Health gone");
        }

        //if (healthImage.fillAmount != PC.Health/100)
        //{
        //    healthImage.fillAmount = PC.Health / 100;
        //    Debug.Log(healthImage.fillAmount + " " + PC.Health/100);
        //}
        //return;
    }

    public void ArmorCheck()
    {

        if (PC.Armor >= 2)
        {
            animOne.SetBool("Destroyed", false);
            animTwo.SetBool("Destroyed", false);
            Debug.Log("Full Armor plates");
        }

        else if (PC.Armor == 1)
        {
            animOne.SetBool("Destroyed", false);
            animTwo.SetBool("Destroyed", true);
            Debug.Log("1st Armor plate destroyed");
        }

        else
        {
            animOne.SetBool("Destroyed", true);
            animTwo.SetBool("Destroyed", true);
            Debug.Log("2nd Armor plate Destroyed");
            HealthBar();
        }
    }

    public void Bullets()
    {
        // bullets.text = " " + PC.AmmoRemaining(ammo).ToString();
    }

    public void setAmmo(int currentType)
    {
        ammoType[currentType].SetActive(true);
        ammoType[(currentType + 1) % 4].SetActive(false);
        ammoType[(currentType + 2) % 4].SetActive(false);
        ammoType[(currentType + 3) % 4].SetActive(false);

        //ammoType[currentType].transform.position = current.transform.position;
        //ammoType[(currentType + 1) % 4].transform.position = pos1.transform.position;
        //ammoType[(currentType + 2) % 4].transform.position = pos2.transform.position;
        //ammoType[(currentType + 3) % 4].transform.position = pos3.transform.position;
        // lightprefab.transform.position = pos3.transform.position;
        Debug.Log("Ammo Changed!");
    }


    public void BulletUpgrade()
    {
        if (slot2.GetComponent<Image>().enabled == true)
        {
            slot3.GetComponent<Image>().enabled = true;
            slot3.GetComponent<Animator>().enabled = true;
        }
        else if (slot1.GetComponent<Image>().enabled == true)
        {
            slot2.GetComponent<Image>().enabled = true;
            slot2.GetComponent<Animator>().enabled = true;
        }
        else
        {
            slot1.GetComponent<Image>().enabled = true;
            slot1.GetComponent<Animator>().enabled = true;
        }
    }


}
