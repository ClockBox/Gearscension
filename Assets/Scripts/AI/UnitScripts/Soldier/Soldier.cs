using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Soldier : AIStateManager
{
	public Transform gunPoint;
    public Transform gunPivot;
    public Collider handCollider;
	public Rigidbody bulletPrefab;
	public float bulletSpeed;
	public float shotInterval;
	public float attackInterval;
	private float shotFrequency;
	private float attackFrequency;
	public float meleeRange;
	public float meleeDamage;
	private GameObject fireBurster;
	private Animator anim;
	public float predictionMagnitude;
	private Vector3 lastPlayerPosition;

	public override void RangedAttack()
	{
		if (!callOnce)
		{
			shotFrequency = shotInterval-0.9f;
			GameObject smoke = Instantiate(smokePrefab, exhaust.transform.position, exhaust.transform.rotation);
			smoke.GetComponent<ParticleSystem>().Emit(1);
			Destroy(smoke, 1f);
			lastPlayerPosition = player.transform.position;
			callOnce = true;
		}
		
		if (shotFrequency >= shotInterval)
		{
            Vector3 bulletDirection = (PredictPosition(lastPlayerPosition, player.transform.position) - gunPoint.transform.position);
            Aim(bulletDirection);

            Rigidbody _bullet;
			_bullet = Instantiate(bulletPrefab, gunPoint.transform.position, gunPoint.transform.rotation);
            _bullet.velocity = bulletDirection.normalized * bulletSpeed;
			Destroy(_bullet.gameObject, 5);
			shotFrequency = 0;
		}
		shotFrequency += Time.deltaTime;
	}

    private void Aim(Vector3 direction)
    {
        Vector3 vertical = Vector3.ProjectOnPlane(direction, transform.right);
        gunPivot.transform.LookAt(transform.position + vertical);
    }

	public override void MeleeAttack()
	{
		if (!callOnce)
		{
			attackFrequency = attackInterval;
			callOnce = true;
		}

        if (!player)
            return;

		if (attackFrequency >= attackInterval)
		{
			if (Vector3.Distance(transform.position, player.transform.position)>5f) 
			{
				pathAgent.speed = 8;
			}

            pathAgent.destination = player.transform.position;
			if (Vector3.Distance(transform.position, player.transform.position) <= meleeRange)
			{
				anim.SetTrigger("Attack");
				fireBurster.GetComponent<ParticleSystem>().Emit(2);
                StartCoroutine(ToggleHandCollider(0.5f, true));
            }
			attackFrequency = 0;
		}
		attackFrequency += Time.deltaTime;
        StartCoroutine(ToggleHandCollider(3));
    }

    private IEnumerator ToggleHandCollider(float waitTime,bool enabled = false)
    {
        yield return new WaitForSeconds(waitTime);
        handCollider.enabled = enabled;
    } 
	

	public override void StartEvents()
    {
		fireBurster = Instantiate(firePrefab, transform.position, transform.rotation);
		fireBurster.transform.parent = transform;
		anim = GetComponent<Animator>();
		
	}

	private Vector3 PredictPosition(Vector3 lastPos, Vector3 currentPos)
	{
		Vector3 target = (currentPos + (currentPos - lastPos).normalized * predictionMagnitude);
        return target + Vector3.up * 1.5f;
	}
}
