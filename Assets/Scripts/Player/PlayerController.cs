using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StateManager))]
[RequireComponent(typeof(IKController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    private static PlayerController m_player;
    private static StateManager m_stateM;
    private static IKController m_ik;
    private static Animator m_anim;
    private static Rigidbody m_rb;

    public GameObject Ragdoll;

    private float elapsedTime = 0;
    private float HookRange = 15;

    //Inputs variables
    const int UP = 0;
    const int DOWN = 1;
    const int STAY = 2;

    private int rightTriggerState = -1;
    private int leftTriggerState = -1;

    //weapon info
    public Transform SwordSheath;
    public Transform GunHolster;
    public Weapon[] weapons;
    private bool[] _hasWeapon = { false, false };

    private int _gunUpgrade = 4;
    private int[] _ammoAmounts = new int[4];
    private BulletType _currentAmmo = BulletType.Electric;

    const int GUN = 0;
    const int SWORD = 1;

    //HUD data
    private bool _isDead;
    private static float _maxHealth = 100;
    private static float _maxArmor = 2;
    private float _currentHealth = _maxHealth;
    private float _currentArmor = _maxArmor;
    private float _armorRecharge = 5.0f;
    private float _damageImmune = 0.5f;

    //Getters & Setters
    public static PlayerController Player
    {
        get { return m_player; }
        set { m_player = value; }
    }
    public static StateManager StateM
    {
        get { return m_stateM; }
        set { m_stateM = value; }
    }
    public static IKController IK
    {
        get { return m_ik; }
        set { m_ik = value; }
    }
    public static Rigidbody rb
    {
        get { return m_rb; }
        set { m_rb = value; }
    }
    public static Animator anim
    {
        get { return m_anim; }
        set { m_anim = value; }
    }

    public int GunUpgrades
    {
        get { return _gunUpgrade; }
    }
    public int AmmoType
    {
        get { return (int)_currentAmmo; }
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
    #endregion

    #region Weapon Function
    public IEnumerator ToggleWeapon(int WeaponType, float toggleTime, float transitionTime)
    {
        _hasWeapon[WeaponType] = !_hasWeapon[WeaponType];
        anim.SetBool("hasSword", _hasWeapon[SWORD]);
        anim.SetBool("aiming", _hasWeapon[GUN]);
        if (WeaponType == 1)
            IK.RightHand.weight = 0;

        yield return new WaitForSeconds(toggleTime);

        Transform weapon = weapons[WeaponType].transform;
        if (!_hasWeapon[WeaponType])
        {
            if (WeaponType == SWORD)
            {
                weapon.parent = SwordSheath;
                weapon.rotation = SwordSheath.rotation;
                weapon.localEulerAngles = new Vector3(0, 0, 90);
                weapon.position = SwordSheath.position;
            }
            else if (WeaponType == GUN)
            {
                weapon.parent = GunHolster;
                weapon.rotation = GunHolster.rotation;
                weapon.position = GunHolster.position;
            }
        }
        else
        {
            if (WeaponType == SWORD)
            {
                weapon.parent = anim.GetBoneTransform(HumanBodyBones.RightHand);
                weapon.localPosition = new Vector3(-0.1f, 0.035f, 0);
                weapon.localEulerAngles = new Vector3(90, 0, 90);
            }
            else if (WeaponType == GUN)
            {
                weapon.parent = anim.GetBoneTransform(HumanBodyBones.LeftHand);
                weapon.localPosition = new Vector3(-0.1f, 0.05f, -0.04f);
                weapon.localEulerAngles = new Vector3(0, -90, 100);
            }
            else Debug.Log("Invalid WeaponType :" + weapon.gameObject.name);
        }
        yield return new WaitForSeconds(transitionTime - toggleTime);
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
    public void UpgradeGun()
    {
        _gunUpgrade++;
        //hud.SendMessage(GunUpgrade(index));
    }

    public GameObject FindHookTarget(string tag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
        GameObject temp = null;
        float closestAngle = 0.5f;
        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 checkDistance = targets[i].transform.position - Player.transform.position;
            if (checkDistance.magnitude < HookRange)
            {
                float checkAngle = (Vector3.Dot(targets[i].transform.position - Player.transform.position, Camera.main.transform.forward));
                if (checkAngle > closestAngle)
                {
                    closestAngle = checkAngle;
                    temp = targets[i];
                }
            }
        }
        return temp;
    }
    #endregion

    #region Health & Armour
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
    private void RechargeArmor()
    {
        if (_currentArmor < _maxArmor && !_isDead)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= _armorRecharge)
            {
                _currentArmor++;
                elapsedTime = 0;
                Debug.Log("Health: " + _currentHealth + ",  \tArmor: " + _currentArmor);
            }
        }
    }
    private void Die()
    {
        _isDead = true;
        DropWeapons();
        Instantiate(Ragdoll, transform.position, transform.rotation);
        Destroy(transform.gameObject);
    }
    #endregion

    //Initialize Player
    private void Awake()
    {
        GameManager.Player = this;
    }

    private void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (!m_player)
            m_player = this;
        else DestroyImmediate(gameObject);

        m_stateM = GetComponent<StateManager>();
        m_ik = GetComponent<IKController>();
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();

        //Initialize states
        m_stateM.State = new UnequipedState(m_stateM, true);

        //Start states
        m_stateM.StartState(m_stateM.State);
    }
    private void Update()
    {
        RightTriggerState();
        LeftTriggerState();

        if (_damageImmune > 0)
            _damageImmune -= Time.deltaTime;

        //testing armor and health
        if (Input.GetKeyDown(KeyCode.Alpha0))
            TakeDamage(10);
        RechargeArmor();

        //Switching Ammo Types
        if (Input.GetButtonDown("Ammo 1") || Input.GetAxis("AmmoAxis Vertical") > 0)
        {
            if (_gunUpgrade >= (int)BulletType.Electric)
                _currentAmmo = BulletType.Electric;
        }
        else if (Input.GetButtonDown("Ammo 2") || Input.GetAxis("AmmoAxis Horizontal") > 0)
        {
            if (_gunUpgrade >= (int)BulletType.Ice)
                _currentAmmo = BulletType.Ice;
        }
        else if (Input.GetButtonDown("Ammo 3") || Input.GetAxis("AmmoAxis Vertical") < 0)
        {
            if (_gunUpgrade >= (int)BulletType.Explosive)
                _currentAmmo = BulletType.Explosive;
        }

        else if (Input.GetButtonDown("Ammo 4") || Input.GetAxis("AmmoAxis Horizontal") < 0)
        {
            if (_gunUpgrade >= (int)BulletType.Magnetic)
                _currentAmmo = BulletType.Magnetic;
        }
    }

    //Input Functions
    private void RightTriggerState()
    {
        if (rightTriggerState == -1)
        {
            if (Input.GetAxisRaw("RightTrigger") > 0)
                rightTriggerState = DOWN;
        }
        else if (rightTriggerState > 0)
        {
            if (Input.GetAxisRaw("RightTrigger") > 0)
                rightTriggerState = STAY;
            else
                rightTriggerState = UP;
        }
        else rightTriggerState = -1;
    }
    private void LeftTriggerState()
    {
        if (leftTriggerState == -1)
        {
            if (Input.GetAxisRaw("LeftTrigger") > 0)
                leftTriggerState = DOWN;
        }
        else if (leftTriggerState > 0)
        {
            if (Input.GetAxisRaw("LeftTrigger") > 0)
                leftTriggerState = STAY;
            else
                leftTriggerState = UP;
        }
        else leftTriggerState = -1;
    }
}
