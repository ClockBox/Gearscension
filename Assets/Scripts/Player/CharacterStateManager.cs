using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(IKController))]
public class CharacterStateManager : MonoBehaviour
{
    CharacterState _PlayerState;
    CharacterState _EquipmentState;

    public GameObject Ragdoll;
    Animator anim;

    float elapsedTime = 0;

    //weapon info
    public Transform SwordSheath;
    public Transform GunHolster;
    public Weapon[] weapons;
    bool[] _hasWeapon = { false, false };

    bool[] _gunUpgrades = new bool[4];
    int[] _ammoAmounts = new int[4];
    int _currentAmmo = Electric;

    const int GUN = 0;
    const int SWORD = 1;

    const int Electric = 0;
    const int Freezing = 1;
    const int Exposive = 2;
    const int mangetic = 3;

    //HUD data
    public float armorRecharge = 5.0f;

    static float _maxHealth = 100;
    static float _maxArmor = 2;
    float _currentHealth = _maxHealth;
    float _currentArmor = _maxArmor;
    float _damageImmune = 0.5f;

    void Start()
    {
        //this should be in gamemanager
        Cursor.lockState = CursorLockMode.Locked;
        
        _PlayerState = new CharacterState(gameObject);
        _PlayerState = new GroundedState();
        _EquipmentState = new EquipmentState();

        if (weapons == null) Debug.Log("No Weapons set");
        if (!SwordSheath) Debug.Log("No SwordSheath set");
        if (!GunHolster) Debug.Log("No GunHolster set");

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        EquipmentState.RightTriggerState();
        EquipmentState.LeftTriggerState();

        if (_damageImmune > 0)
            _damageImmune -= Time.deltaTime;

        //testing armor and health
        if (Input.GetKeyDown(KeyCode.Alpha0))
            TakeDamage(50);
        RechargeArmor();

        //Switching Ammo Types
        if (Input.GetButtonDown("Ammo 1") || Input.GetAxis("AmmoAxis Vertical") > 0)
        {
            if (_gunUpgrades[Electric])
                _currentAmmo = Electric;
        }
        else if (Input.GetButtonDown("Ammo 2") || Input.GetAxis("AmmoAxis Horizontal") > 0)
        {
            if (_gunUpgrades[Freezing])
                _currentAmmo = Freezing;
        }
        else if (Input.GetButtonDown("Ammo 3") || Input.GetAxis("AmmoAxis Vertical") < 0)
        {
            if (_gunUpgrades[Exposive])
                _currentAmmo = Exposive;
        }

        else if (Input.GetButtonDown("Ammo 4") || Input.GetAxis("AmmoAxis Horizontal") < 0)
        {
            if (_gunUpgrades[mangetic])
                _currentAmmo = mangetic;
        }
    //}

    ////State Functions
    //private void FixedUpdate()
    //{
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
            Debug.Log(newState);
            StartCoroutine(currentState.ExitState());
            return newState;
        }
        else return currentState;
    }
    
    //Weapon Functions
    public void ToggleWeapon(int WeaponType)
    {
        Transform weapon = weapons[WeaponType].transform;
        if (_hasWeapon[WeaponType])
        {
            if (WeaponType == SWORD)
                weapon.parent = SwordSheath;
            else if (WeaponType == GUN)
                weapon.parent = GunHolster;
        }
        else
        {
            if (WeaponType == SWORD)
            {
                weapon.parent = anim.GetBoneTransform(HumanBodyBones.RightHand);
                weapon.localPosition = new Vector3(-0.1f, 0.035f, 0.02f);
                weapon.rotation = weapon.parent.rotation * new Quaternion(1, 0, 0, 1);
            }
            else if (WeaponType == GUN)
            {
                weapon.parent = anim.GetBoneTransform(HumanBodyBones.LeftHand);
                weapon.localPosition = new Vector3(-0.1f, 0.05f, -0.04f);
            }
            else Debug.Log("Invalid WeaponType :" + weapon.gameObject.name);
        }
        _hasWeapon[WeaponType] = !_hasWeapon[WeaponType];
        anim.SetBool("hasSword", _hasWeapon[SWORD]);
    }

    public void DropWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.GetComponent<BoxCollider>().enabled = true;
            weapons[i].gameObject.AddComponent<Rigidbody>();
            weapons[i].transform.parent = null;
        }
    }

    public void UpgradeGun(int index)
    {
        _gunUpgrades[index] = true;
        //hud.SendMessage(GunUpgrade(index));
    }

    //Health and armor functions
    public void TakeDamage(float damage)
    {
        if (_damageImmune <= 0)
        {
            _damageImmune = 0.5f;

            if (_currentArmor > 0)
                _currentArmor--;
            else
                _currentHealth -= damage;

            Debug.Log("Health: " + _currentHealth + ",  \tArmor: " + _currentArmor);

            if (Health <= 0)
                Die();
        }
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
                if (elapsedTime >= armorRecharge)
                {
                    _currentArmor++;
                    elapsedTime = 0;
                    Debug.Log("Health: " + _currentHealth + ",  \tArmor: " + _currentArmor);
                }
            }
        }
    }

    void Die()
    {
        DropWeapons();
        Instantiate(Ragdoll, transform.position, transform.rotation);
        Destroy(transform.gameObject);
    }

    //Getters and setters
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

    public bool HasWeapon(int index)
    {
        return _hasWeapon[index];
    }
}
