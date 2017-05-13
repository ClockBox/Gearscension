using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(IKController))]
public class CharacterStateManager : MonoBehaviour
{
    static CharacterState _PlayerState;
    static CharacterState _EquipmentState;

    static float _maxHealth = 100;
    static float _currentHealth = _maxHealth;
    static float _Armor = 2;

    void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _PlayerState = new CharacterState(gameObject);
        _PlayerState = new GroundedState();
        _EquipmentState = new EquipmentState();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        EquipmentState.RightTriggerState();
        EquipmentState.LeftTriggerState();

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
        if (_Armor > 0)
            Armor--;
        else
            Health -= damage;

        if (Health <= 0)
            Die();
    }

    void Die()
    {
        //GameManager.SendMessage.playereDead;
    }

    public float Health
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }

    public float Armor
    {
        get { return _Armor; }
        set { _Armor = value; }
    }
}
