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
	private Vector3 hopDirection;
	private Vector3 hopTarget;

	public override void StartEvents() { }

	public override void RangedAttack()
	{
		if (!callOnce)
        {
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
		if (!callOnce)
		{
			pathAgent.enabled = false;
			GetComponent<Rigidbody>().isKinematic = false;
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

			LayerMask mask = LayerMask.GetMask("Character");
			mask = ~mask;
			hopTarget = transform.position;
			RaycastHit hit;
			if (Physics.Raycast(gunPoint.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, mask))
			{
				hopDirection = transform.forward;
				hopTarget = hit.transform.position;
			}

			RaycastHit hit2;
			if (Physics.Raycast(gunPoint.position, transform.TransformDirection(Vector3.back), out hit2, Mathf.Infinity, mask))
			{
				if (Vector3.Distance(hit2.transform.position, transform.position) > Vector3.Distance(hopTarget, transform.position))
				{
					hopDirection = transform.forward * -1;
					hopTarget = hit2.transform.position;
				}
			}

			RaycastHit hit3;
			if (Physics.Raycast(gunPoint.position, transform.TransformDirection(Vector3.right), out hit3, Mathf.Infinity, mask))
			{
				if (Vector3.Distance(hit3.transform.position, transform.position) > Vector3.Distance(hopTarget, transform.position))
				{
					hopDirection = transform.right;
					hopTarget = hit3.transform.position;
				}
			}

			RaycastHit hit4;
			if (Physics.Raycast(gunPoint.position, transform.TransformDirection(Vector3.right), out hit4, Mathf.Infinity, mask))
			{
				if (Vector3.Distance(hit4.transform.position, transform.position) > Vector3.Distance(hopTarget, transform.position))
				{
					hopDirection = transform.right*-1;
					hopTarget = hit3.transform.position;
				}
			}
			GetComponent<Rigidbody>().AddForce(hopDirection * 450);
			GetComponent<Rigidbody>().AddForce(transform.up * 550);
			callOnce = true;
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
