using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenadier : AIStateManager {

	public Transform gunPoint;
	public float launchAngle;
	public Rigidbody grenadePrefab;


	public override void RangedAttack()
	{

		Vector3 displacement = player.transform.position - transform.position;
		Rigidbody rb = Instantiate(grenadePrefab, gunPoint.position, gunPoint.rotation) as Rigidbody;
		rb.velocity = CalculateVelocityArc(launchAngle, displacement);

	}
	public override void MeleeAttack()
	{
	}
	public override void AlertOthers()
	{
	}
	public override void Die()
	{
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
