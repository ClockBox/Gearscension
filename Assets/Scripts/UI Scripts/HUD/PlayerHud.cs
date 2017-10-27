using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class PlayerHud : MonoBehaviour
{
    PlayerController PC;
    
    private int ammo;

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
    private GameObject ammoReelMid;
    private Animator animReelMid;
    [SerializeField] private GameObject ammoReelTop;
    private Animator animReelTop;
    [SerializeField] private GameObject ammoReelBot;
    private Animator animReelBot;

    public GameObject[] ammoType = new GameObject[4];

    void Awake()
    {
        GameManager.Hud = this;
        PC = GameManager.Player;

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

        ammoReelMid = GameObject.Find("/Gear Hud/AmmoTypes/AmmoReel Middle");
        if (ammoReelMid != null)
        {
            animReelMid = ammoReelMid.GetComponent<Animator>();
        }

        ammoReelTop = GameObject.Find("/Gear Hud/AmmoTypes/AmmoReel Top");
        if (ammoReelTop != null)
        {
            animReelTop = ammoReelTop.GetComponent<Animator>();
        }

        ammoReelBot = GameObject.Find("/Gear Hud/AmmoTypes/AmmoReel Bottom");
        if (ammoReelBot != null)
        {
            animReelBot = ammoReelBot.GetComponent<Animator>();
        }

        healthBar = new GameObject[]
        {
            healthZero, healthOne, healthTwo, healthThree, healthFour, healthFive
        };
    }

    void Update()
    {
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
        }

        else if (healthCount == 4)
        {
            healthFour.SetActive(true);

            healthZero.SetActive(true);
            healthOne.SetActive(false);
            healthTwo.SetActive(false);
            healthThree.SetActive(false);

            healthFive.SetActive(false);
        }

        else if (healthCount == 3)
        {
            healthThree.SetActive(true);

            healthZero.SetActive(false);
            healthOne.SetActive(false);
            healthTwo.SetActive(false);

            healthFour.SetActive(false);
            healthFive.SetActive(false);
        }

        else if (healthCount == 2)
        {
            healthTwo.SetActive(true);

            healthZero.SetActive(false);
            healthOne.SetActive(false);

            healthThree.SetActive(false);
            healthFour.SetActive(false);
            healthFive.SetActive(false);
        }

        else if (healthCount == 1)
        {
            healthOne.SetActive(true);

            healthZero.SetActive(false);

            healthTwo.SetActive(false);
            healthThree.SetActive(false);
            healthFour.SetActive(false);
            healthFive.SetActive(false);
        }

        else
        {
            healthZero.SetActive(true);

            healthOne.SetActive(false);
            healthTwo.SetActive(false);
            healthThree.SetActive(false);
            healthFour.SetActive(false);
            healthFive.SetActive(false);
        }
    }

    public void ArmorCheck()
    {

        if (PC.Armor >= 2)
        {
            animOne.SetBool("Destroyed", false);
            animTwo.SetBool("Destroyed", false);
        }

        else if (PC.Armor == 1)
        {
            animTwo.SetBool("Destroyed", true);
        }

        else
        {
            animOne.SetBool("Destroyed", true);
            animTwo.SetBool("Destroyed", true);
            HealthBar();
        }
    }

    public void SetAmmo(int currentType)
    {
        ammoType[currentType].SetActive(true);
        ammoType[(currentType + 1) % 4].SetActive(false);
        ammoType[(currentType + 2) % 4].SetActive(false);
        ammoType[(currentType + 3) % 4].SetActive(false);

        if ((currentType + 1) % 4 <= PC.GunUpgrades)
        {
            // Set Top Open
            animReelTop.SetBool("Active", true);
        }
        else
        {
            // Set Top Closed.
            animReelTop.SetBool("Active", false);
        }
        if ((currentType + 3) % 4 <= PC.GunUpgrades)
        {
            // Set Bottom Open
            animReelBot.SetBool("Active", true);
        }
        else
        {
            // Set Bottom Closed.
            animReelBot.SetBool("Active", false);
        }
    }

    public void BulletUpgrade()
    {
        animReelMid.SetTrigger("Active");
        SetAmmo(ammo);
    }

    public void AddDisplay(string passedString)
    {
        whiteText.text = "" + passedString;
        textBox.SetActive(true);
    }

    public void RemoveDisplay()
    {
        textBox.SetActive(false);
    }
}
