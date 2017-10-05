using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AmmoSwap : MonoBehaviour
{

    // Int
    private int ammo;

    PlayerController CSM;

    [Header("Main Hud")]
    [SerializeField] private GameObject healtOne;
    [SerializeField] private GameObject healtTwo;
    [SerializeField] private GameObject healtThree;
    [SerializeField] private GameObject healtFour;
    [SerializeField] private GameObject healtFive;
    [SerializeField] private GameObject healtSix;

    private GameObject[] healthBar;

    [SerializeField] private GameObject armorPieceOne;
    private Animator animOne;
    private bool armorOneTrigger = true;

    [SerializeField] private GameObject armorPieceTwo;
    private Animator animTwo;
    private bool armorTwoTrigger = true;

    [Header("Ammo Hud")]
    [SerializeField] private Image slot1;
    [SerializeField] private Image slot2;
    [SerializeField] private Image slot3;

    // Other
    [Header("Ammo")]
    public Image current;
    public Text bullets;
    [SerializeField] private Image pos1;
    [SerializeField] private Image pos2;
    [SerializeField] private Image pos3;

    public GameObject[] sprites = new GameObject[4];

    // Use this for initialization
    void Awake()
    {
        CSM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        armorPieceOne = GameObject.FindGameObjectWithTag("Armor 1");
        if (armorPieceOne != null)
        {
            animOne = GetComponent<Animator>();
        }

        armorPieceTwo = GameObject.FindGameObjectWithTag("Armor2");
        if (armorPieceTwo != null)
        {
            animTwo = GetComponent<Animator>();
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



        //if (ammo != CSM.AmmoType)
        //{
        //    ammo = CSM.AmmoType;
        //    setAmmo(ammo);
        //}
        //HealthBar();
        ArmorCheck();
        //Bullets();
    }

    private void HealthBar()
    {
        Mathf.RoundToInt(CSM.Health / 20);

        if()
        {

        }

        //if (healthImage.fillAmount != CSM.Health/100)
        //{
        //    healthImage.fillAmount = CSM.Health / 100;
        //    Debug.Log(healthImage.fillAmount + " " + CSM.Health/100);
        //}
        //return;
    }

    public void ArmorCheck()
    {

        if (CSM.Armor >= 2)
        {
            animOne.SetBool("Destroyed", false);
            animTwo.SetBool("Destroyed", false);
            animOne.SetTrigger("armorTrigger");
            animTwo.SetTrigger("armorTrigger");
        }
        else if (CSM.Armor == 1)
        {
            animOne.SetBool("Destroyed", false);
            animTwo.SetBool("Destroyed", true);
            animOne.SetTrigger("armorTrigger");
            animTwo.SetTrigger("armorTrigger");
        }
        else if (CSM.Armor == 0)
        {
            animOne.SetBool("Destroyed", true);
            animTwo.SetBool("Destroyed", true);
            animOne.SetTrigger("armorTrigger");
            animTwo.SetTrigger("armorTrigger");
            HealthBar();
        }
    }

    public void Bullets()
    {
        bullets.text = " " + CSM.AmmoRemaining(ammo).ToString();
    }

    public void setAmmo(int currentType)
    {
        sprites[currentType].transform.position = current.transform.position;
        sprites[(currentType + 1) % 4].transform.position = pos1.transform.position;



        sprites[(currentType + 2) % 4].transform.position = pos2.transform.position;
        sprites[(currentType + 3) % 4].transform.position = pos3.transform.position;
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
