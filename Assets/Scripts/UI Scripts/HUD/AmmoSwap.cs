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
    private GameObject healthBar;
    private Image healthImage;
    private GameObject armorBar;
    private Image armorImage;

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

        healthBar = GameObject.FindGameObjectWithTag("Health Bar");
        if (healthBar != null)
        {
            healthImage = healthBar.GetComponent<Image>();
        }

        armorBar = GameObject.FindGameObjectWithTag("Armor Bar");
        if (armorBar != null)
        {
            armorImage = healthBar.GetComponent<Image>();
        }

    }


    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
            { BulletUpgrade(); }

        if (ammo != CSM.AmmoType)
        {
            ammo = CSM.AmmoType;
            setAmmo(ammo);
        }
        HealthBar();
        ArmorBar();
        Bullets();
    }

    private void HealthBar()
    {

        if (healthImage.fillAmount != CSM.Health/100)
        {
            healthImage.fillAmount = CSM.Health / 100;
            Debug.Log(healthImage.fillAmount + " " + CSM.Health/100);
        }
        return;
    }

    private void ArmorBar()
    {
        if (armorImage.fillAmount != CSM.Armor/2)
        {
            armorImage.fillAmount = CSM.Armor / 2;
            Debug.Log(armorImage.fillAmount + " " + CSM.Armor/2);
        }
        return;
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
