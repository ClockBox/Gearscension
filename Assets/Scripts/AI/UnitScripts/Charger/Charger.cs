using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : AIStateManager
{
	public float waitTime;
	public BoxCollider chargeShield;
	public float chargeForce;

    public float shieldForce;
    public float shieldTime;

    private bool hitSomthing = false;

	public override void StartEvents() { }

	public override void MeleeAttack()
	{
	}

	public override void RangedAttack()
	{
		if (!callOnce)
		{
			pathAgent.speed = 0;
			pathAgent.angularSpeed = 0;
			pathAgent.enabled = false;
            StartCoroutine(Charge(player.transform.position, waitTime));
			callOnce = true;
		}
	}

	IEnumerator Charge(Vector3 target, float delayTime)
	{
        chargeShield.enabled = true;
        Vector3 dir = (target - transform.position).normalized;
        dir.y = transform.position.y;

        float elapsedTime = 0;
        while (elapsedTime <= delayTime)
        {
            transform.LookAt(transform.position + dir);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        rb.AddForce(dir * chargeForce, ForceMode.Impulse);

        elapsedTime = 0;
        while (elapsedTime <= 0.5f)
        {
            transform.LookAt(transform.position + dir);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        chargeShield.enabled = false;
        rb.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Player.gameObject)
        {
            PlayerState.grounded = false;
            GameManager.Player.TakeDamage(20);
            Rigidbody playerRb = other.attachedRigidbody;
            playerRb.AddForce(Vector3.up / 2 * playerRb.mass * shieldForce, ForceMode.Impulse);
            playerRb.AddForce(transform.forward * playerRb.mass * shieldForce, ForceMode.Impulse);
        }
    }
}
