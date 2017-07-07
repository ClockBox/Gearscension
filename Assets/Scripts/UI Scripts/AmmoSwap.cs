using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AmmoSwap : MonoBehaviour
{

    PlayerController CSM;

    [Header("Health")]
    public Image healthBar;

    [Header("Armor")]
    public Image armorBar;

    // Other
    [Header("Weapons")]
    public Image current;
    public Image pos1;
    public Image pos2;
    public Image pos3;
    private int ammo;
    public Text bullets;

    //public sprite fire; 2
    //public sprite ice; 1
    //public sprite mask (0);
    //public sprite magno; 3
    public GameObject[] sprites = new GameObject[4];


    public GameObject lightprefab;

    // Use this for initialization
    void Start()
    {
        CSM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // change later when get another bullet.
        // magno = mask;

        lightprefab.GetComponent<Animator>();
        lightprefab.GetComponent<Animation>();
        
    }


    // Update is called once per frame
    void Update()
    {
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

        if (healthBar.fillAmount != CSM.Health/100)
        {
            healthBar.fillAmount = CSM.Health / 100;
            // Debug.Log(healthBar.fillAmount + " " + CSM.Health/100);
        }
        return;
    }

    private void ArmorBar()
    {
        if (armorBar.fillAmount != CSM.Armor/2)
        {
            armorBar.fillAmount = CSM.Armor / 2;
            // Debug.Log(armorBar.fillAmount + " " + CSM.Armor/2);
        }
        return;
    }

    public void Bullets()
    {
        bullets.text = " " + CSM.AmmoRemaining(ammo).ToString();
    }

    //public void Fire()
    //{
    //    current.sprite = fire;
    //    pos1.sprite = ice;
    //    pos2.sprite = mask;
    //    lightprefab.transform.position = pos2.transform.position;
    //    pos3.sprite = magno;
    //}

    //public void Ice()
    //{
    //    current.sprite = ice;
    //    pos1.sprite = mask;
    //    lightprefab.transform.position = pos1.transform.position;
    //    pos2.sprite = magno;
    //    pos3.sprite = fire;
    //}

    //public void Lighting()
    //{
    //    current.sprite = mask;
    //    lightprefab.transform.position = current.transform.position;
    //    pos1.sprite = magno;
    //    pos2.sprite = fire;
    //    pos3.sprite = ice;
    //}

    //public void Magno()
    //{
    //    current.sprite = magno;
    //    pos1.sprite = fire;
    //    pos2.sprite = ice;
    //    pos3.sprite = mask;
    //    lightprefab.transform.position = pos3.transform.position;
    //}

    public void setAmmo(int currentType)
    {
        sprites[currentType].transform.position = current.transform.position;
        sprites[(currentType + 1) % 4].transform.position = pos1.transform.position;
        sprites[(currentType + 2) % 4].transform.position = pos2.transform.position;
        sprites[(currentType + 3) % 4].transform.position = pos3.transform.position;
        // lightprefab.transform.position = pos3.transform.position;
        Debug.Log("Ammo Changed!");
    }
}
