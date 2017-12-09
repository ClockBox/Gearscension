using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeShield : MonoBehaviour
{
	public float shieldTime;
    public float force;
	private void Start()
	{
        Destroy(gameObject, shieldTime);
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == GameManager.Player.gameObject)
		{
            PlayerState.grounded = false;
            GameManager.Player.TakeDamage(20);
            Rigidbody player = other.attachedRigidbody;
            player.AddForce(((Vector3.up / 2) + transform.forward) * player.mass * force, ForceMode.Impulse);
        }
	}
}
