using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AmmoSwap : MonoBehaviour
{

    CharacterStateManager CSM;

    [Header("Health")]
    public Image healthBar;
    // public float maxHealth;
    // private float currentHealth;

    [Header("Armor")]
    public Image armorBar;
    // public float maxArmor;
    // private float currentArmor;

    // Other
    [Header("Weapons")]
    public Image current;
    public Image pos1;
    public Image pos2;
    public Image pos3;
    public Image[] images = new Image[4];

    public Sprite fire;
    public Sprite ice;
    public Sprite mask;
    public Sprite magno;
    public Sprite[] sprites = new Sprite[4];


    public GameObject lightprefab;

    // Use this for initialization
    void Start()
    {
        CSM = GetComponent<CharacterStateManager>();
        // currentHealth = maxHealth;
        // currentArmor = maxArmor;

        // change later when get another bullet.
        magno = mask;

        lightprefab.GetComponent<Animator>();
        lightprefab.GetComponent<Animation>();
        
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void HealthBar()
    {
        if (CSM.Health != healthBar.fillAmount)
        {
            healthBar.fillAmount = CSM.Health;
            Debug.Log("Health Bar Filled.");
        }
        else
            return;
    }

    private void ArmorBar()
    {
        if (CSM.Armor != armorBar.fillAmount)
        {
            armorBar.fillAmount = CSM.Armor;
            Debug.Log("Armor Bar Filled.");
        }
        else
        return;
    }

    public void Fire()
    {
        current.sprite = fire;
        pos1.sprite = ice;
        pos2.sprite = mask;
        lightprefab.transform.position = pos2.transform.position;
        pos3.sprite = magno;
    }

    public void Ice()
    {
        current.sprite = ice;
        pos1.sprite = mask;
        lightprefab.transform.position = pos1.transform.position;
        pos2.sprite = magno;
        pos3.sprite = fire;
    }

    public void Lighting()
    {
        current.sprite = mask;
        lightprefab.transform.position = current.transform.position;
        pos1.sprite = magno;
        pos2.sprite = fire;
        pos3.sprite = ice;
    }

    public void Magno()
    {
        current.sprite = magno;
        pos1.sprite = fire;
        pos2.sprite = ice;
        pos3.sprite = mask;
        lightprefab.transform.position = pos3.transform.position;
    }

    public void setAmmo(int currentType)
    {
        current.sprite = sprites[currentType];
        pos1.sprite = sprites[(currentType + 1) % 4];
        pos2.sprite = sprites[(currentType + 2) % 4];
        pos3.sprite = sprites[(currentType + 3) % 4];
        lightprefab.transform.position = pos3.transform.position;
    }
}
