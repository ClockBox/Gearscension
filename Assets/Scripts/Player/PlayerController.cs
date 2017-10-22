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

    // trigger inputs variables
    private InputAxis rightTriggerState = new InputAxis("RightTrigger",AxisType.Trigger);
    private InputAxis leftTriggerState = new InputAxis("LeftTrigger", AxisType.Trigger);
    private InputAxis ammoAxis = new InputAxis("AmmoAxis", AxisType.Axis);

    //weapon info
    public Transform SwordSheath;
    public Transform GunHolster;
    public Weapon[] weapons;

    [SerializeField, Range(-1,3)]
    private int gunUpgrade = -1;
    private int[] ammoAmounts = new int[4];
    private int ammoType = 0;

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

    public int AmmoType
    {
        get { return ammoType; }
    }
    public int AmmoRemaining(int index)
    {
        if (index < ammoAmounts.Length)
            return ammoAmounts[index];
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

    public int GunUpgrades
    {
        get { return gunUpgrade; }
    }
    
    public InputAxis RightTrigger
    {
        get { return rightTriggerState; }
    }
    public InputAxis LeftTrigger
    {
        get { return leftTriggerState; }
    }
    public InputAxis AmmoAxis
    {
        get { return ammoAxis; }
    }
    #endregion

    #region Weapon Function
    public void DropWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
            weapons[i].gameObject.AddComponent<Rigidbody>();
            weapons[i].transform.parent = null;
        }
    }

    public void PickupGun()
    {
        weapons[0].gameObject.SetActive(true);
        UpgradeGun(0);
    }

    public void UpgradeGun(int upgrade)
    {
        gunUpgrade = upgrade;
        GameManager.Instance.Hud.BulletUpgrade();
    }

    public GameObject FindHookTarget(string tag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
        GameObject temp = null;
        float closestAngle = 0.8f;
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
        if (!GameManager.Instance.Player)
            GameManager.Instance.Player = this;
        else Destroy(gameObject);
    }

    private void Start ()
    {
        if (!m_player)
            m_player = this;
        else
        {
            DestroyImmediate(gameObject);
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;

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
        if (_damageImmune > 0)
            _damageImmune -= Time.deltaTime;

        //testing armor and health
        if (Input.GetKeyDown(KeyCode.Alpha0))
            TakeDamage(10);
        RechargeArmor();

        //Switching Ammo Types
        if (Input.GetButtonDown("Ammo 1"))
        {
            if (gunUpgrade >= (int)BulletType.Electric)
                ammoType = (int)BulletType.Electric;
        }
        else if (Input.GetButtonDown("Ammo 2"))
        {
            if (gunUpgrade >= (int)BulletType.Ice)
                ammoType = (int)BulletType.Ice;
        }
        else if (Input.GetButtonDown("Ammo 3"))
        {
            if (gunUpgrade >= (int)BulletType.Explosive)
                ammoType = (int)BulletType.Explosive;
        }

        else if (Input.GetButtonDown("Ammo 4"))
        {
            if (gunUpgrade >= (int)BulletType.Magnetic)
                ammoType = (int)BulletType.Magnetic;
        }

        if (ammoAxis.Down)
        {
            ammoType = (ammoType + ammoAxis.RawValue) % 4;
            if (ammoType < 0) ammoType += 4;
        }
    }
}
