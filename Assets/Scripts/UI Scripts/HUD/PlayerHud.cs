using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class PlayerHud : MonoBehaviour
{

    PlayerController PC;

    // Int
    private int ammo;

    // String
    private string passedString;

    [Header("Main Hud")]
    [SerializeField]
    private GameObject healthZero;
    [SerializeField] private GameObject healthOne;
    [SerializeField] private GameObject healthTwo;
    [SerializeField] private GameObject healthThree;
    [SerializeField] private GameObject healthFour;
    [SerializeField] private GameObject healthFive;

    private GameObject[] healthBar;

    private GameObject armorPieceOne;
    private Animator animOne;

    private GameObject armorPieceTwo;
    private Animator animTwo;

    [SerializeField] private GameObject textBox;
    [SerializeField] private Text whiteText;
    [SerializeField] private Text blackText;

    [Header("Ammo Hud")]
    [SerializeField]
    private GameObject ammoReel;
    private Animator animReel;

    public GameObject[] ammoType = new GameObject[4];

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

        ammoReel = GameObject.Find("/Gear Hud/AmmoTypes/AmmoReel");
        if (ammoReel != null)
        {
            animReel = ammoReel.GetComponent<Animator>();
        }

        healthBar = new GameObject[]
        {
            healthZero, healthOne, healthTwo, healthThree, healthFour, healthFive
        };
    }

    private void Start()
    {

        blackText.text = "";
        DisplayText();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        { BulletUpgrade(); }

        if (ammo != PC.AmmoType)
        {
            ammo = PC.AmmoType;
            SetAmmo(ammo);
        }

        ArmorCheck();
    }

    private void HealthBar()
    {
        int healthCount = Mathf.RoundToInt(PC.Health / 20);

        if (healthCount >= 5)
        {
            healthFive.SetActive(true);

            healthZero.SetActive(false);
            healthOne.SetActive(false);
            healthTwo.SetActive(false);
            healthThree.SetActive(false);
            healthFour.SetActive(false);
            Debug.Log("Health Set to full");
        }

        else if (healthCount == 4)
        {
            healthFour.SetActive(true);

            healthZero.SetActive(true);
            healthOne.SetActive(false);
            healthTwo.SetActive(false);
            healthThree.SetActive(false);

            healthFive.SetActive(false);
            Debug.Log("Four bars of health Remaining");
        }

        else if (healthCount == 3)
        {
            healthThree.SetActive(true);

            healthZero.SetActive(false);
            healthOne.SetActive(false);
            healthTwo.SetActive(false);

            healthFour.SetActive(false);
            healthFive.SetActive(false);
            Debug.Log("Three bars of health remianing");
        }

        else if (healthCount == 2)
        {
            healthTwo.SetActive(true);

            healthZero.SetActive(false);
            healthOne.SetActive(false);

            healthThree.SetActive(false);
            healthFour.SetActive(false);
            healthFive.SetActive(false);
            Debug.Log("Two bars of health remianing");
        }

        else if (healthCount == 1)
        {
            healthOne.SetActive(true);

            healthZero.SetActive(false);

            healthTwo.SetActive(false);
            healthThree.SetActive(false);
            healthFour.SetActive(false);
            healthFive.SetActive(false);
            Debug.Log("One bars of health remianing");
        }

        else
        {
            healthZero.SetActive(true);

            healthOne.SetActive(false);
            healthTwo.SetActive(false);
            healthThree.SetActive(false);
            healthFour.SetActive(false);
            healthFive.SetActive(false);
            Debug.Log("Player has Died");
        }
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

    public void DisplayText()
    {
        // whiteText.text = "" + PC.passedString;
        // blackText.text = "" + PC.passedString;
    }

    public void SetAmmo(int currentType)
    {

        ammoType[currentType].SetActive(true);
        ammoType[(currentType + 1) % 4].SetActive(false);
        ammoType[(currentType + 2) % 4].SetActive(false);
        ammoType[(currentType + 3) % 4].SetActive(false);
        Debug.Log("Ammo type changed!");
    }

    public void BulletUpgrade()
    {
        animReel.SetTrigger("Active");
        Debug.Log("Ammo Hud Activated.");
    }

    public void AddDislplay(string description)
    {
        DisplayText();

        textBox.SetActive(true);
    }

    public void RemoveDisplay()
    {
        textBox.SetActive(false);
    }
}
