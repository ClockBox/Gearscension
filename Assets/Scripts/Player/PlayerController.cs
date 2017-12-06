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

    private static bool m_paused;

    public GameObject Ragdoll;

    private float elapsedTime = 0;
    private float HookRange = 15;
    private CinemachineController cC;
    // Audio
    private AudioSource m_SFX;
    private AudioSource m_Voice;

    // trigger inputs variables
    private InputAxis rightTriggerState = new InputAxis("RightTrigger",AxisType.Trigger);
    private InputAxis leftTriggerState = new InputAxis("LeftTrigger", AxisType.Trigger);
    private InputAxis ammoAxis = new InputAxis("AmmoAxis", AxisType.Axis);

    //weapon info
    public Transform SwordSheath;
    public Transform GunHolster;
    public Transform AimPoint;
    public Weapon[] weapons;

    [SerializeField, Range(-1,3)]
    private int gunUpgrade = -1;
    private int[] ammoAmounts = new int[4];
    private static int ammoType = 0;

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
    public static Rigidbody RB
    {
        get { return m_rb; }
        set { m_rb = value; }
    }
    public static Animator Anim
    {
        get { return m_anim; }
        set { m_anim = value; }
    }

    public bool Paused
    {
        get { return m_paused; }
    }
    public void PausePlayer()
    {
        RB.velocity = Vector3.zero;
        m_paused = true;
    }
    public void UnPausePlayer()
    {
        m_paused = false;
    }

    public AudioSource SFX
    {
        get { return m_SFX; }
        set { m_SFX = value; }
    }
    public AudioSource Voice
    {
        get { return m_Voice; }
        set { m_Voice = value; }
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

    public void UpgradeGun(int upgrade)
    {
        GameManager.Player.weapons[0].gameObject.SetActive(true);
        GameManager.Player.gunUpgrade = upgrade;
        ammoType = upgrade;
        GameManager.Hud.BulletUpgrade();
    }

    public GameObject FindHookTarget(string tag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
        GameObject temp = null;
        float closestAngle = 0.8f;
        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 checkDistance = targets[i].transform.position - transform.position;
            if (checkDistance.magnitude < HookRange && Vector3.Dot(targets[i].transform.forward, Camera.main.transform.forward) > 0.5f)
            {
                float checkAngle = Vector3.Dot((targets[i].transform.position - transform.position).normalized, Camera.main.transform.forward);
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
            elapsedTime = 0;

            if (_currentArmor > 0)
                _currentArmor--;
            else
                _currentHealth -= damage;

            if (Health <= 0)
                Die();
            Debug.Log("Health: " + _currentHealth + "  Armour: " + _currentArmor);
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
                Debug.Log("ArmourRegen: " + _currentArmor);
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

    public void PlaySound(string soundName)
    {
        GameManager.Instance.AudioManager.AudioPlayer = m_SFX;
        GameManager.Instance.AudioManager.playAudio(soundName);
    }

    //Initialize Player
    private void Awake()
    {
        if (!GameManager.Player)
        {
            GameManager.Player = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("Here");
            StopAllCoroutines();
            DestroyImmediate(gameObject);
        }

        if (!GameManager.Player)
            return;

        m_stateM = GetComponent<StateManager>();
        m_ik = GetComponent<IKController>();
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();

        m_SFX = transform.GetChild(2).GetComponent<AudioSource>();
        m_Voice = transform.GetChild(3).GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        Debug.Log("Player Destroyed");
    }

    private void Start ()
    {
        cC = GetComponent<CinemachineController>();

        Debug.Log("Player Start", this);
        //Initialize state Machine
        m_stateM.State = new UnequipedState(m_stateM, true);
        m_stateM.StartState(m_stateM.State);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cowbell"))
        {
            GameManager.Instance.AudioManager.AudioPlayer = SFX;
            GameManager.Instance.AudioManager.playAudio("sfxcowbell");
        }

        if (_damageImmune > 0)
            _damageImmune -= Time.deltaTime;

        //testing armor and health
        if (Input.GetKeyDown(KeyCode.Alpha0))
            TakeDamage(10);
        RechargeArmor();

        if (AimPoint) AimPoint.rotation = CameraController.MainCamera.transform.rotation * new Quaternion(0, 0.7071068f, 0, 0.7071068f);

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
