using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenadier : AIStateManager {

	public Transform gunPoint;
	public float launchAngle;
	public Rigidbody icePrefab;
	public Rigidbody magnetPrefab;
	public Rigidbody explosionPrefab;
	public float shotInterval;
	private float shotFrequency;
	private int choice;
	private Rigidbody grenadePrefab;

	public override void StartEvents() { }

	public override void RangedAttack()
	{
		
		if (!callOnce) {
			choice = UnityEngine.Random.Range(0, 3);
			shotFrequency = shotInterval;
			switch (choice) {
				case 0:
					grenadePrefab = explosionPrefab;
					break;
				case 1:
					grenadePrefab = icePrefab;
					break;
				case 2:
					grenadePrefab = magnetPrefab;
					break;
			}
			callOnce = true;
		}
		if (shotFrequency >= shotInterval)
		{
			Vector3 displacement = player.transform.position - transform.position;
			Rigidbody rb = Instantiate(grenadePrefab, gunPoint.position, gunPoint.rotation) as Rigidbody;
			rb.velocity = CalculateVelocityArc(launchAngle, displacement);

			shotFrequency = 0;
		}
		shotFrequency += Time.deltaTime;

	}
	public override void MeleeAttack()
	{

	}

	public override void Die()
	{
		if (isAlive)
		{
			isAlive = false;
			Destroy(gameObject, 1f);
		}
	}
	

	private Vector3 CalculateVelocityArc(float angle, Vector3 displacement)
	{
		float height = displacement.y;
		displacement.y = 0;
		float horizontalDistance = displacement.magnitude;
		float angleRad = angle * Mathf.Deg2Rad;
		displacement.y = horizontalDistance * Mathf.Tan(angleRad);
		horizontalDistance += height / Mathf.Tan(angleRad);
		float velocityStart = Mathf.Sqrt(horizontalDistance * Physics.gravity.magnitude / Mathf.Sin(2 * angleRad));
		return velocityStart * displacement.normalized;

	}
}
