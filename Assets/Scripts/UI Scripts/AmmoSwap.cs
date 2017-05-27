using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AmmoSwap : MonoBehaviour
{

    CharacterStateManager CSM;

    [Header("Health")]
    public Image healthBar;
    public float maxHealth;
    private float currentHealth;

    [Header("Armor")]
    public Image armorBar;
    public float maxArmor;
    private float currentArmor;


    // Int


    // Float


    // Other
    [Header("Weapons")]
    public Image current;
    public Image pos1;
    public Image pos2;
    public Image pos3;

    public Sprite fire;
    public Sprite ice;
    public Sprite mask;
    public Sprite magno;

    public GameObject lightprefab;

    // Use this for initialization
    void Start()
    {
        // Set AmmoType
        // CSM.AmmoType = Fire();

        Fire();


        currentHealth = maxHealth;
        currentArmor = maxArmor;

        // change later when get another bullet.
        magno = mask;

        lightprefab.GetComponent<Animator>();
        lightprefab.GetComponent<Animation>();
        
    }


    // Update is called once per frame
    void Update()
    {
        /*
        if(CSM.AmmoType == 0)
        {
            Lighting();
        }
        else if (CSM.AmmoType == 1)
        {
            Ice();
        }
        else if (CSM.AmmoType == 2)
        {
            Fire();
        }
        else if (CSM.AmmoType == 3)
        {
            Magno();
        }
        */

        if (Input.GetButtonDown("1"))
        {
            Fire();
        }
        else if (Input.GetButtonDown("2"))
        {
            Ice();
        }
        else if (Input.GetButtonDown("3"))
        {
            Lighting();
        }
        else if (Input.GetButtonDown("4"))
        {
            Magno();
        }
        else { return; }

    }

    private void HealthBar()
    {
        if (CSM.Health != healthBar.fillAmount)
        {
            healthBar.fillAmount = CSM.Health;
        }
        else
            return;
    }

    private void ArmorBar()
    {
        if (CSM.Health != armorBar.fillAmount)
        {
            armorBar.fillAmount = CSM.Armor;
        }
        else
        return;
    }

    void Fire()
    {
        current.sprite = fire;
        pos1.sprite = ice;
        pos2.sprite = mask;
        lightprefab.transform.position = pos2.transform.position;
        pos3.sprite = magno;
    }

    void Ice()
    {
        current.sprite = ice;
        pos1.sprite = mask;
        lightprefab.transform.position = pos1.transform.position;
        pos2.sprite = magno;
        pos3.sprite = fire;
    }

    void Lighting()
    {
        current.sprite = mask;
        lightprefab.transform.position = current.transform.position;
        pos1.sprite = magno;
        pos2.sprite = fire;
        pos3.sprite = ice;
    }

    void Magno()
    {
        current.sprite = magno;
        pos1.sprite = fire;
        pos2.sprite = ice;
        pos3.sprite = mask;
        lightprefab.transform.position = pos3.transform.position;
    }

}
