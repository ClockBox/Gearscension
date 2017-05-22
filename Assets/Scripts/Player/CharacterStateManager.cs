using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(IKController))]
public class CharacterStateManager : MonoBehaviour
{
    static CharacterState _PlayerState;
    static CharacterState _EquipmentState;

    public GameObject Ragdoll;

    float elapsedTime = 0;

    //HUD data
    static float _maxHealth = 100;
    static float _currentHealth = _maxHealth;
    static float _maxArmor = 2;
    static float _currentArmor = _maxArmor;
    public float ArmorRecharge = 5.0f;

    //Gun Data
    const int Electric = 0;
    const int Freezing = 1;
    const int Exposive = 2;
    const int mangetic = 3;

    int _currentAmmo = Electric;
    bool[] _gunUpgrades = new bool[4];
    static int[] _ammoAmounts = new int[4];

    void Start ()
    {
        //this should be in gamemanager.Start()
        Cursor.lockState = CursorLockMode.Locked;
        //initialize States
        _PlayerState = new CharacterState(gameObject);
        _PlayerState = new GroundedState();
        _EquipmentState = new EquipmentState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        //testing armor and health
        if (Input.GetKeyDown(KeyCode.Alpha0))
            TakeDamage(50);
        RechargeArmor();

        //Switching Ammo Types
        if (Input.GetButtonDown("Ammo 1") || Input.GetAxis("AmmoAxis Vertical") > 0)
            _currentAmmo = Electric;
        else if (Input.GetButtonDown("Ammo 2") || Input.GetAxis("AmmoAxis Horizontal") > 0)
            _currentAmmo = Freezing;
        else if(Input.GetButtonDown("Ammo 3") || Input.GetAxis("AmmoAxis Vertical") < 0)
            _currentAmmo = Exposive;
        else if (Input.GetButtonDown("Ammo 4") || Input.GetAxis("AmmoAxis Horizontal") < 0)
            _currentAmmo = mangetic;

        EquipmentState.RightTriggerState();
        EquipmentState.LeftTriggerState();
    }

    private void FixedUpdate()
    {
        _PlayerState = HandleStateChange(_PlayerState, _PlayerState.UpdateState());
        _EquipmentState = HandleStateChange(_EquipmentState, _EquipmentState.UpdateState());
    }

    private void OnAnimatorIK(int layerIndex)
    {
        _PlayerState.UpdateIK();
        _EquipmentState.UpdateIK();
    }

    void OnTriggerEnter(Collider other)
    {
        _PlayerState = HandleStateChange(_PlayerState, _PlayerState.OnTriggerEnter(other));
        _EquipmentState = HandleStateChange(_EquipmentState, _EquipmentState.OnTriggerEnter(other));
    }

    void OnTriggerStay(Collider other)
    {
        _PlayerState = HandleStateChange(_PlayerState, _PlayerState.OnTriggerStay(other));
        _EquipmentState = HandleStateChange(_EquipmentState, _EquipmentState.OnTriggerStay(other));
    }

    CharacterState HandleStateChange (CharacterState currentState, CharacterState newState)
    {
        if (newState != null)
        {
            //Debug.Log(newState);
            currentState.ExitState();
            return newState;
        }
        else return currentState;
    }

    public void ToggleWeapon(int weaponIndex)
    {
        EquipmentState.ToggleWeapon(weaponIndex);
    }

    public void TakeDamage(float damage)
    {
        if (_currentArmor > 0)
            _currentArmor--;
        else
            _currentHealth -= damage;

        Debug.Log("Health: " + _currentHealth + "\tArmor: " + _currentArmor);

        if (Health <= 0)
            Die();
    }

    private void UpgradeGun(int index)
    {
        _gunUpgrades[index] = true;
    }

    private void RechargeArmor(int amount = 0)
    {
        if (amount != 0)
            _currentArmor += amount;
        else
        {
            if (_currentArmor < _maxArmor)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= ArmorRecharge)
                {
                    _currentArmor++;
                    elapsedTime = 0;
                    Debug.Log("Health: " + _currentHealth + "\tArmor: " + _currentArmor);
                }
            }
        }
    }

    void Die()
    {
        Instantiate(Ragdoll, transform.position, transform.rotation);
        //GameManager.SendMessage.playerDead;
        Destroy(transform.gameObject);
    }

    public bool GunUpgrades(int index)
    {
        if (index < _gunUpgrades.Length)
            return _gunUpgrades[index];
        else return false;
    }

    public int AmmoType
    {
        get { return _currentAmmo; }
    }

    public int AmmoRemaining(int index)
    {
        if (index < _ammoAmounts.Length)
            return _ammoAmounts[index];
        else return 0;
    }

    public float Health
    {
        get { return _currentHealth; }
    }

    public float Armor
    {
        get { return _currentArmor; }
    }
}
